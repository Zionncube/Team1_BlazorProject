namespace FinanceTrackerApp.Models;

/// <summary>
/// Represents a monthly spending limit for a specific category.
/// Used to help users control expenses and track overspending.
/// </summary>
public class Budget
{
    /// <summary>Unique identifier for the budget record</summary>
    public Guid BudgetId { get; set; } = Guid.NewGuid();

    /// <summary>User that owns this budget</summary>
    public string UserId { get; set; } = "demo-user";

    /// <summary>Category the budget applies to</summary>
    public Guid CategoryId { get; set; }

    /// <summary>Maximum allowed spending for the month</summary>
    public decimal MonthlyLimit { get; set; }

    /// <summary>Budget year</summary>
    public int Year { get; set; } = DateTime.Today.Year;

    /// <summary>Budget month</summary>
    public int Month { get; set; } = DateTime.Today.Month;
}