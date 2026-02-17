using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

/// <summary>
/// In-memory fallback implementation for goal storage.
/// Useful for local demos without persistence dependencies.
/// </summary>
public class InMemoryGoalsStore : IGoalsStore
{
    private readonly List<SavingsGoal> _goals = new();
    private readonly List<GoalContribution> _contributions = new();

    /// <summary>
    /// Seeds demo data for quick local testing.
    /// </summary>
    public InMemoryGoalsStore()
    {
        var goal1 = new SavingsGoal
        {
            Title = "Save for Laptop",
            Description = "New laptop for coding & projects",
            TargetAmount = 5000,
            CurrentAmount = 1350,
            TargetDate = DateTime.Today.AddMonths(4),
            Color = "#2BEF83"
        };
        var goal2 = new SavingsGoal
        {
            Title = "Emergency Fund",
            Description = "3 months safety buffer",
            TargetAmount = 12000,
            CurrentAmount = 4200,
            TargetDate = DateTime.Today.AddMonths(10),
            Color = "#4EE3FF"
        };

        _goals.Add(goal1);
        _goals.Add(goal2);

        _contributions.Add(new GoalContribution
        {
            GoalId = goal1.GoalId,
            Amount = 350,
            Date = DateTime.Today.AddDays(-6),
            Note = "Side project payout"
        });
        _contributions.Add(new GoalContribution
        {
            GoalId = goal1.GoalId,
            Amount = 200,
            Date = DateTime.Today.AddDays(-2),
            Note = "Freelance"
        });
        _contributions.Add(new GoalContribution
        {
            GoalId = goal2.GoalId,
            Amount = 500,
            Date = DateTime.Today.AddDays(-4),
            Note = "Monthly transfer"
        });
    }

    /// <summary>Returns all seeded goals.</summary>
    public Task<List<SavingsGoal>> GetGoalsAsync(string userId, CancellationToken ct = default)
    {
        return Task.FromResult(_goals.ToList());
    }

    /// <summary>Returns all seeded contributions.</summary>
    public Task<List<GoalContribution>> GetContributionsAsync(string userId, CancellationToken ct = default)
    {
        return Task.FromResult(_contributions.ToList());
    }

    /// <summary>Adds a goal or updates an existing one.</summary>
    public Task SaveGoalAsync(string userId, SavingsGoal goal, CancellationToken ct = default)
    {
        var existing = _goals.FirstOrDefault(g => g.GoalId == goal.GoalId);
        if (existing is null)
        {
            _goals.Add(goal);
        }
        else
        {
            existing.Title = goal.Title;
            existing.Description = goal.Description;
            existing.TargetAmount = goal.TargetAmount;
            existing.CurrentAmount = goal.CurrentAmount;
            existing.TargetDate = goal.TargetDate;
            existing.Color = goal.Color;
            existing.IsCompleted = goal.IsCompleted;
        }
        return Task.CompletedTask;
    }

    /// <summary>Deletes a goal by id.</summary>
    public Task DeleteGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        _goals.RemoveAll(g => g.GoalId == goalId);
        return Task.CompletedTask;
    }

    /// <summary>Adds one contribution in memory.</summary>
    public Task AddContributionAsync(string userId, GoalContribution contribution, CancellationToken ct = default)
    {
        _contributions.Add(contribution);
        return Task.CompletedTask;
    }

    /// <summary>Deletes contributions linked to the goal.</summary>
    public Task DeleteContributionsForGoalAsync(string userId, Guid goalId, CancellationToken ct = default)
    {
        _contributions.RemoveAll(c => c.GoalId == goalId);
        return Task.CompletedTask;
    }
}
