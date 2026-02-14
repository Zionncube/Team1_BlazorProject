using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services
{
    public class BudgetService
    {
        private readonly FinanceDbContext _db;

        public BudgetService(FinanceDbContext db)
        {
            _db = db;
        }

        public async Task<List<Budget>> GetBudgetsForMonthAsync(string userId, int year, int month)
        {
            return await _db.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId && b.Year == year && b.Month == month)
                .OrderBy(b => b.Category!.Name)
                .ToListAsync();
        }

        public async Task<Budget?> GetBudgetByIdAsync(Guid budgetId)
        {
            return await _db.Budgets
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BudgetId == budgetId);
        }

        public async Task<Budget?> GetBudgetByCategoryAsync(string userId, Guid categoryId, int year, int month)
        {
            return await _db.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && 
                                         b.CategoryId == categoryId && 
                                         b.Year == year && 
                                         b.Month == month);
        }

        public async Task CreateBudgetAsync(Budget budget)
        {
            _db.Budgets.Add(budget);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateBudgetAsync(Budget budget)
        {
            _db.Budgets.Update(budget);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBudgetAsync(Guid budgetId)
        {
            var budget = await _db.Budgets.FindAsync(budgetId);
            if (budget != null)
            {
                _db.Budgets.Remove(budget);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetCategoriesAsync(string userId)
        {
            return await _db.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
