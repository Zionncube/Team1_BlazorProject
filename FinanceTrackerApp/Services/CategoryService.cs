using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services;

/// <summary>
/// Handles CRUD operations for user categories.
/// </summary>
public class CategoryService
{
    private readonly FinanceDbContext _db;

    /// <summary>
    /// Creates the service with the EF Core context.
    /// </summary>
    public CategoryService(FinanceDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns categories owned by a user, ordered by name.
    /// </summary>
    public async Task<List<Category>> GetByUserAsync(string userId)
    {
        return await _db.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Returns one category only if it belongs to the provided user.
    /// </summary>
    public async Task<Category?> GetByIdAsync(Guid categoryId, string userId)
    {
        return await _db.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.UserId == userId);
    }

    /// <summary>
    /// Creates a new category for a user.
    /// </summary>
    public async Task AddAsync(Category category, string userId)
    {
        category.CategoryId = category.CategoryId == Guid.Empty ? Guid.NewGuid() : category.CategoryId;
        category.UserId = userId;
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Updates category name/color when ownership matches.
    /// </summary>
    public async Task UpdateAsync(Category category, string userId)
    {
        var existing = await GetByIdAsync(category.CategoryId, userId);
        if (existing is null) return;
        existing.Name = category.Name;
        existing.Color = category.Color;
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a category when ownership matches.
    /// </summary>
    public async Task DeleteAsync(Guid categoryId, string userId)
    {
        var existing = await GetByIdAsync(categoryId, userId);
        if (existing is null) return;
        _db.Categories.Remove(existing);
        await _db.SaveChangesAsync();
    }
}
