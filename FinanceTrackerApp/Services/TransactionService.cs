using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services
{
    public class TransactionService
    {
        private readonly FinanceDbContext _db;

        public TransactionService(FinanceDbContext db)
        {
            _db = db;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(string userId)
        {
            return await _db.Transactions
                            .Where(t => t.UserId == userId)
                            .OrderByDescending(t => t.Date)
                            .ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        // Compatibility wrappers used by branch pages (AddTransaction/EditTransaction)
        public async Task AddAsync(Transaction transaction)
        {
            await AddTransactionAsync(transaction);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }

        // Compatibility wrappers used by branch pages (AddTransaction/EditTransaction)
        public async Task UpdateAsync(Transaction transaction)
        {
            await UpdateTransactionAsync(transaction);
        }

        public async Task<Transaction?> GetByIdAsync(Guid transactionId)
        {
            return await _db.Transactions.FindAsync(transactionId);
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync(string userId)
        {
            var transactions = await _db.Transactions
                .Where(t => t.UserId == userId)
                .ToListAsync();

            var now = DateTime.Today;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var monthly = transactions
                .Where(t => t.Date >= monthStart && t.Date < nextMonthStart)
                .ToList();

            var totalIncomeThisMonth = monthly
                .Where(t => string.Equals(t.Type, "income", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            var totalExpensesThisMonth = monthly
                .Where(t => string.Equals(t.Type, "expense", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            var totalIncomeAllTime = transactions
                .Where(t => string.Equals(t.Type, "income", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            var totalExpensesAllTime = transactions
                .Where(t => string.Equals(t.Type, "expense", StringComparison.OrdinalIgnoreCase))
                .Sum(t => t.Amount);

            var topCategory = transactions
                .Where(t => string.Equals(t.Type, "expense", StringComparison.OrdinalIgnoreCase))
                .GroupBy(t => string.IsNullOrWhiteSpace(t.Category) ? "Uncategorized" : t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Total)
                .FirstOrDefault()
                ?.Category ?? "N/A";

            return new DashboardSummary
            {
                TotalBalance = totalIncomeAllTime - totalExpensesAllTime,
                TotalIncomeThisMonth = totalIncomeThisMonth,
                TotalExpensesThisMonth = totalExpensesThisMonth,
                TopCategory = topCategory,
                TransactionsCount = transactions.Count
            };
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            var tx = await _db.Transactions.FindAsync(transactionId);
            if (tx != null)
            {
                _db.Transactions.Remove(tx);
                await _db.SaveChangesAsync();
            }
        }
    }
}
