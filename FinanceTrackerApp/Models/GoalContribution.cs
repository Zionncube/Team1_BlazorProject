namespace FinanceTrackerApp.Models;

/// <summary>
/// Represents a deposit made toward a savings goal.
/// Each contribution increases the goal's progress.
/// </summary>
public class GoalContribution
{
    /// <summary>Unique identifier for the contribution</summary>
    public Guid ContributionId { get; set; } = Guid.NewGuid();

    /// <summary>The goal this contribution belongs to</summary>
    public Guid GoalId { get; set; }

    /// <summary>User that made the contribution</summary>
    public string UserId { get; set; } = "demo-user";

    /// <summary>Amount added to the goal</summary>
    public decimal Amount { get; set; }

    /// <summary>Date of contribution</summary>
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>Optional note describing the contribution</summary>
    public string? Note { get; set; }
}