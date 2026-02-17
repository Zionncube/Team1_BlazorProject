namespace FinanceTrackerApp.Models;

/// <summary>
/// Template for a repeating transaction definition.
/// </summary>
public class RecurringTransaction
{
    /// <summary>Unique identifier for the recurring item.</summary>
    public Guid RecurringTransactionId { get; set; } = Guid.NewGuid();
    /// <summary>User that owns this recurring template.</summary>
    public string UserId { get; set; } = "demo-user";
    /// <summary>Amount applied per recurrence.</summary>
    public decimal Amount { get; set; }
    /// <summary>Category assigned to generated transactions.</summary>
    public string Category { get; set; } = "General";
    /// <summary>User-facing description.</summary>
    public string Description { get; set; } = "";
    /// <summary>Transaction type: income or expense.</summary>
    public string Type { get; set; } = "expense";
    /// <summary>Recurrence cadence (for example: monthly).</summary>
    public string Frequency { get; set; } = "monthly";
    /// <summary>When the recurrence begins.</summary>
    public DateTime StartDate { get; set; } = DateTime.Today;
    /// <summary>Whether this template is active.</summary>
    public bool IsActive { get; set; } = true;
}
