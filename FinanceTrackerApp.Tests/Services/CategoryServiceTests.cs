using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using FinanceTrackerApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTrackerApp.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task CategoryService_CRUD_WorksPerUser()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new FinanceDbContext(options);
        var service = new CategoryService(db);
        const string user1 = "u1";
        const string user2 = "u2";

        var cat = new Category { Name = "Food", Color = "#ff0000" };
        await service.AddAsync(cat, user1);

        var user1Cats = await service.GetByUserAsync(user1);
        var user2Cats = await service.GetByUserAsync(user2);
        Assert.Single(user1Cats);
        Assert.Empty(user2Cats);

        var saved = user1Cats.First();
        saved.Name = "Groceries";
        await service.UpdateAsync(saved, user1);

        var updated = await service.GetByIdAsync(saved.CategoryId, user1);
        Assert.NotNull(updated);
        Assert.Equal("Groceries", updated!.Name);

        await service.DeleteAsync(saved.CategoryId, user1);
        var afterDelete = await service.GetByUserAsync(user1);
        Assert.Empty(afterDelete);
    }
}
