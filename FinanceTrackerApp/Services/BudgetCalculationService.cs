using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services
{
    public class BudgetSummary
    {
        public decimal TotalBudgeted { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal RemainingBudget => TotalBudgeted - TotalSpent;
        public int OverspendingCount { get; set; }
        public List<BudgetAlert> Alerts { get; set; } = new();
    }

    public class BudgetAlert
    {
        public Guid BudgetId { get; set; }
        public string CategoryName { get; set; } = "";
        public decimal BudgetLimit { get; set; }
        public decimal CurrentSpending { get; set; }
        public decimal OverspentAmount => CurrentSpending - BudgetLimit;
        public double PercentageOver => BudgetLimit > 0 ? (double)((CurrentSpending - BudgetLimit) / BudgetLimit * 100) : 0;
        public double PercentageUsed => BudgetLimit > 0 ? (double)(CurrentSpending / BudgetLimit * 100) : 0;
    }

    public class BudgetProgressData
    {
        public Guid BudgetId { get; set; }
        public string CategoryName { get; set; } = "";
        public string CategoryColor { get; set; } = "#2BEF83";
        public decimal BudgetLimit { get; set; }
        public decimal CurrentSpending { get; set; }
        public decimal Remaining => BudgetLimit - CurrentSpending;
        public double PercentageUsed => BudgetLimit > 0 ? (double)(CurrentSpending / BudgetLimit * 100) : 0;
        public bool IsOverspent => CurrentSpending > BudgetLimit;
    }

    public class BudgetCalculationService
    {
        private readonly FinanceDbContext _db;

        public BudgetCalculationService(FinanceDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> CalculateCategorySpendingAsync(string userId, Guid categoryId, int year, int month)
        {
            var categoryName = await GetCategoryNameAsync(categoryId);
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _db.Transactions
                .Where(t => t.UserId == userId &&
                           t.Category == categoryName &&
                           t.Type == "expense" &&
                           t.Date >= startDate &&
                           t.Date <= endDate)
                .SumAsync(t => t.Amount);
        }

        private async Task<string> GetCategoryNameAsync(Guid categoryId)
        {
            var category = await _db.Categories.FindAsync(categoryId);
            return category?.Name ?? "General";
        }

        public async Task<List<BudgetProgressData>> GetBudgetProgressAsync(string userId, int year, int month)
        {
            var budgets = await _db.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && b.Year == year && b.Month == month)
                .ToListAsync();

            var progressList = new List<BudgetProgressData>();

            foreach (var budget in budgets)
            {
                var spending = await CalculateCategorySpendingAsync(userId, budget.CategoryId, year, month);

                progressList.Add(new BudgetProgressData
                {
                    BudgetId = budget.BudgetId,
                    CategoryName = budget.Category?.Name ?? "General",
                    CategoryColor = budget.Category?.Color ?? "#2BEF83",
                    BudgetLimit = budget.MonthlyLimit,
                    CurrentSpending = spending
                });
            }

            return progressList.OrderByDescending(p => p.PercentageUsed).ToList();
        }

        public async Task<BudgetSummary> GetBudgetSummaryAsync(string userId, int year, int month)
        {
            var progressData = await GetBudgetProgressAsync(userId, year, month);

            var summary = new BudgetSummary
            {
                TotalBudgeted = progressData.Sum(p => p.BudgetLimit),
                TotalSpent = progressData.Sum(p => p.CurrentSpending),
                OverspendingCount = progressData.Count(p => p.IsOverspent),
                Alerts = progressData
                    .Where(p => p.IsOverspent)
                    .Select(p => new BudgetAlert
                    {
                        BudgetId = p.BudgetId,
                        CategoryName = p.CategoryName,
                        BudgetLimit = p.BudgetLimit,
                        CurrentSpending = p.CurrentSpending
                    })
                    .ToList()
            };

            return summary;
        }

        public async Task<List<BudgetAlert>> GetOverspendingAlertsAsync(string userId, int year, int month)
        {
            var summary = await GetBudgetSummaryAsync(userId, year, month);
            return summary.Alerts;
        }
    }
}
