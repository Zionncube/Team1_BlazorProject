using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services;

public class CategoryService
{
    private readonly FinanceDbContext _db;

    public CategoryService(FinanceDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetByUserAsync(string userId)
    {
        return await _db.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId, string userId)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.UserId == userId);
    }

    public async Task AddAsync(Category category, string userId)
    {
        category.CategoryId = category.CategoryId == Guid.Empty ? Guid.NewGuid() : category.CategoryId;
        category.UserId = userId;
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category, string userId)
    {
        var existing = await GetByIdAsync(category.CategoryId, userId);
        if (existing is null) return;
        existing.Name = category.Name;
        existing.Color = category.Color;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid categoryId, string userId)
    {
        var existing = await GetByIdAsync(categoryId, userId);
        if (existing is null) return;
        _db.Categories.Remove(existing);
        await _db.SaveChangesAsync();
    }
}
