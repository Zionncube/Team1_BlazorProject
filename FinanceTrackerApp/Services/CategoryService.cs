using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services
{
    public class CategoryService
    {
        private readonly FinanceDbContext _context;

        public CategoryService(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.UserId == userId);
        }

        public async Task<Category?> GetCategoryByNameAsync(string name, string userId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && c.UserId == userId);
        }

        public async Task<Category> CreateCategoryAsync(string userId, string name, string color)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                Color = color,
                CreatedDate = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Guid categoryId, string userId, string name, string color)
        {
            var category = await GetCategoryByIdAsync(categoryId, userId);
            if (category == null)
                throw new InvalidOperationException($"Category with ID {categoryId} not found");

            category.Name = name;
            category.Color = color;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategoryAsync(Guid categoryId, string userId)
        {
            var category = await GetCategoryByIdAsync(categoryId, userId);
            if (category == null)
                throw new InvalidOperationException($"Category with ID {categoryId} not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryInUseAsync(Guid categoryId, string userId)
        {
            return await _context.Budgets
                .AnyAsync(b => b.CategoryId == categoryId && b.UserId == userId);
        }
    }
}
