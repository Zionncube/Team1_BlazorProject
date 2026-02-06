namespace FinanceTrackerApp.Models;

public class SavingsGoal
{
    public Guid GoalId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime? TargetDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public string Color { get; set; } = "#2BEF83";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
