using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using FinanceTrackerApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FinanceTrackerApp.Tests.Services;

public class LocalGoalsStoreTests
{
    [Fact]
    public async Task LocalGoalsStore_SaveAndReadGoal_UsesUserIsolation()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new FinanceDbContext(options);
        var store = new LocalGoalsStore(db);

        var g1 = new SavingsGoal
        {
            Title = "Laptop",
            TargetAmount = 5000,
            CurrentAmount = 500,
            Color = "#2BEF83"
        };
        var g2 = new SavingsGoal
        {
            Title = "Travel",
            TargetAmount = 8000,
            CurrentAmount = 800,
            Color = "#4EE3FF"
        };

        await store.SaveGoalAsync("u1", g1);
        await store.SaveGoalAsync("u2", g2);

        var u1Goals = await store.GetGoalsAsync("u1");
        var u2Goals = await store.GetGoalsAsync("u2");

        Assert.Single(u1Goals);
        Assert.Single(u2Goals);
        Assert.Equal("Laptop", u1Goals[0].Title);
        Assert.Equal("Travel", u2Goals[0].Title);
    }

    [Fact]
    public async Task LocalGoalsStore_AddAndDeleteContributionsForGoal_Works()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new FinanceDbContext(options);
        var store = new LocalGoalsStore(db);
        const string userId = "u1";
        var goalId = Guid.NewGuid();

        await store.SaveGoalAsync(userId, new SavingsGoal
        {
            GoalId = goalId,
            Title = "Emergency Fund",
            TargetAmount = 10000,
            CurrentAmount = 0
        });

        await store.AddContributionAsync(userId, new GoalContribution
        {
            GoalId = goalId,
            Amount = 200,
            Date = DateTime.Today
        });
        await store.AddContributionAsync(userId, new GoalContribution
        {
            GoalId = goalId,
            Amount = 300,
            Date = DateTime.Today
        });

        var beforeDelete = await store.GetContributionsAsync(userId);
        Assert.Equal(2, beforeDelete.Count(c => c.GoalId == goalId));

        await store.DeleteContributionsForGoalAsync(userId, goalId);

        var afterDelete = await store.GetContributionsAsync(userId);
        Assert.DoesNotContain(afterDelete, c => c.GoalId == goalId);
    }
}
