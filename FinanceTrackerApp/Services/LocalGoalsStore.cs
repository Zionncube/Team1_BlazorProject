using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services;

public class LocalGoalsStore : IGoalsStore
{
    private readonly FinanceDbContext _db;

    public LocalGoalsStore(FinanceDbContext db)
    {
        _db = db;
    }

    public async Task<List<SavingsGoal>> GetGoalsAsync(string userId, CancellationToken ct = default)
    {
        return await _db.SavingsGoals
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<GoalContribution>> GetContributionsAsync(string userId, CancellationToken ct = default)
    {
        return await _db.GoalContributions
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.Date)
            .ToListAsync(ct);
    }

    public async Task SaveGoalAsync(string userId, SavingsGoal goal, CancellationToken ct = default)
    {
        goal.UserId = userId;
        if (goal.GoalId == Guid.Empty)
            goal.GoalId = Guid.NewGuid();
        if (goal.CreatedAt == default)
            goal.CreatedAt = DateTime.Now;

        var existing = await _db.SavingsGoals
            .FirstOrDefaultAsync(g => g.GoalId == goal.GoalId && g.UserId == userId, ct);

        if (existing is null)
        {
            _db.SavingsGoals.Add(goal);
        }
        else
        {
            existing.Title = goal.Title;
            existing.Description = goal.Description;
            existing.TargetAmount = goal.TargetAmount;
            existing.CurrentAmount = goal.CurrentAmount;
            existing.TargetDate = goal.TargetDate;
            existing.IsCompleted = goal.IsCompleted;
            existing.Color = goal.Color;
        }
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        var goal = await _db.SavingsGoals
            .FirstOrDefaultAsync(g => g.GoalId == goalId && g.UserId == userId, ct);
        if (goal is null) return;
        _db.SavingsGoals.Remove(goal);
        await _db.SaveChangesAsync(ct);
    }

    public async Task AddContributionAsync(string userId, GoalContribution contribution, CancellationToken ct = default)
    {
        if (contribution.ContributionId == Guid.Empty)
            contribution.ContributionId = Guid.NewGuid();
        contribution.UserId = userId;

        _db.GoalContributions.Add(contribution);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteContributionsForGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        var items = await _db.GoalContributions
            .Where(c => c.UserId == userId && c.GoalId == goalId)
            .ToListAsync(ct);
        if (!items.Any()) return;
        _db.GoalContributions.RemoveRange(items);
        await _db.SaveChangesAsync(ct);
    }
}
