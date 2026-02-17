namespace FinanceTrackerApp.Models;

/// <summary>
/// Aggregate values displayed on the dashboard.
/// </summary>
public class DashboardSummary
{
    /// <summary>Income minus expense across all transactions.</summary>
    public decimal TotalBalance { get; set; }
    /// <summary>Total income in the current month.</summary>
    public decimal TotalIncomeThisMonth { get; set; }
    /// <summary>Total expense in the current month.</summary>
    public decimal TotalExpensesThisMonth { get; set; }
    /// <summary>Most expensive category in current period.</summary>
    public string TopCategory { get; set; } = "N/A";
    /// <summary>Total number of transactions for the user.</summary>
    public int TransactionsCount { get; set; }
}
