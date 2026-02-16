using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using FinanceTrackerApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTrackerApp.Tests.Services;

public class TransactionServiceTests
{
    [Fact]
    public async Task GetDashboardSummaryAsync_ReturnsExpectedTotals()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new FinanceDbContext(options);
        var userId = "u1";
        var now = DateTime.Today;

        db.Transactions.AddRange(
            new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = userId,
                Amount = 1000,
                Type = "income",
                Category = "Salary",
                Description = "Salary",
                Date = now
            },
            new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = userId,
                Amount = 200,
                Type = "expense",
                Category = "Food",
                Description = "Groceries",
                Date = now
            },
            new Transaction
            {
                TransactionId = Guid.NewGuid(),
                UserId = userId,
                Amount = 50,
                Type = "expense",
                Category = "Food",
                Description = "Snacks",
                Date = now
            }
        );
        await db.SaveChangesAsync();

        var service = new TransactionService(db);
        var summary = await service.GetDashboardSummaryAsync(userId);

        Assert.Equal(750, summary.TotalBalance);
        Assert.Equal(1000, summary.TotalIncomeThisMonth);
        Assert.Equal(250, summary.TotalExpensesThisMonth);
        Assert.Equal("Food", summary.TopCategory);
        Assert.Equal(3, summary.TransactionsCount);
    }
}
