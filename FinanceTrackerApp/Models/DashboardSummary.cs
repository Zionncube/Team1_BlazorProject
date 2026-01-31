namespace FinanceTrackerApp.Models;

public class DashboardSummary
{
    public decimal TotalBalance { get; set; }
    public decimal TotalIncomeThisMonth { get; set; }
    public decimal TotalExpensesThisMonth { get; set; }
    public string TopCategory { get; set; } = "N/A";
    public int TransactionsCount { get; set; }
}
