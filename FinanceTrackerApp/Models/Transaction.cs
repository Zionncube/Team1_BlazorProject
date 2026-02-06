namespace FinanceTrackerApp.Models;

public class Transaction
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user"; // temporary for UI stage
    public decimal Amount { get; set; }
    public string Category { get; set; } = "General";
    public DateTime Date { get; set; } = DateTime.Today;
    public string Description { get; set; } = "";
    // "income" or "expense"
    public string Type { get; set; } = "expense";
}
