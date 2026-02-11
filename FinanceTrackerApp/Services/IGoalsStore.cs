using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

public interface IGoalsStore
{
    Task<List<SavingsGoal>> GetGoalsAsync(string userId, CancellationToken ct = default);
    Task<List<GoalContribution>> GetContributionsAsync(string userId, CancellationToken ct = default);
    Task SaveGoalAsync(string userId, SavingsGoal goal, CancellationToken ct = default);
    Task DeleteGoalAsync(string userId, Guid goalId, CancellationToken ct = default);
    Task AddContributionAsync(string userId, GoalContribution contribution, CancellationToken ct = default);
    Task DeleteContributionsForGoalAsync(string userId, Guid goalId, CancellationToken ct = default);
}
