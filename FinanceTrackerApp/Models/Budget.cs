namespace FinanceTrackerApp.Models;

public class Budget
{
    public Guid BudgetId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user";
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Year { get; set; } = DateTime.Today.Year;
    public int Month { get; set; } = DateTime.Today.Month;
}
