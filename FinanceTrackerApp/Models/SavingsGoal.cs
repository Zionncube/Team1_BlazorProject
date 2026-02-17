namespace FinanceTrackerApp.Models;

/// <summary>
/// Represents a user's long-term savings objective.
/// Tracks progress toward a target amount.
/// </summary>
public class SavingsGoal
{
    /// <summary>Unique identifier for the goal</summary>
    public Guid GoalId { get; set; } = Guid.NewGuid();

    /// <summary>User that owns the goal</summary>
    public string UserId { get; set; } = "demo-user";

    /// <summary>Name of the savings goal (e.g. Car, Laptop)</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional details about the goal</summary>
    public string? Description { get; set; }

    /// <summary>Total amount required</summary>
    public decimal TargetAmount { get; set; }

    /// <summary>Amount saved so far</summary>
    public decimal CurrentAmount { get; set; }

    /// <summary>Target completion date</summary>
    public DateTime? TargetDate { get; set; }

    /// <summary>Marks goal as finished</summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>UI display color</summary>
    public string Color { get; set; } = "#2BEF83";

    /// <summary>Creation timestamp</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}