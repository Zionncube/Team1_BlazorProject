namespace FinanceTrackerApp.Models;

public class RecurringTransaction
{
    public Guid RecurringTransactionId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "General";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "expense";
    public string Frequency { get; set; } = "monthly";
    public DateTime StartDate { get; set; } = DateTime.Today;
    public bool IsActive { get; set; } = true;
}
