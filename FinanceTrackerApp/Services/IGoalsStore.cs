using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

/// <summary>
/// Abstraction for persisting savings goals and contributions.
/// </summary>
public interface IGoalsStore
{
    /// <summary>Returns all goals for a user.</summary>
    Task<List<SavingsGoal>> GetGoalsAsync(string userId, CancellationToken ct = default);
    /// <summary>Returns all contributions for a user.</summary>
    Task<List<GoalContribution>> GetContributionsAsync(string userId, CancellationToken ct = default);
    /// <summary>Creates or updates a goal.</summary>
    Task SaveGoalAsync(string userId, SavingsGoal goal, CancellationToken ct = default);
    /// <summary>Deletes one goal.</summary>
    Task DeleteGoalAsync(string userId, Guid goalId, CancellationToken ct = default);
    /// <summary>Adds one contribution entry.</summary>
    Task AddContributionAsync(string userId, GoalContribution contribution, CancellationToken ct = default);
    /// <summary>Deletes all contributions for a goal.</summary>
    Task DeleteContributionsForGoalAsync(string userId, Guid goalId, CancellationToken ct = default);
}
