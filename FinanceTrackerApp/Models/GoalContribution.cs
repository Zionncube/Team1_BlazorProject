namespace FinanceTrackerApp.Models;

public class GoalContribution
{
    public Guid ContributionId { get; set; } = Guid.NewGuid();
    public Guid GoalId { get; set; }
    public string UserId { get; set; } = "demo-user";
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string? Note { get; set; }
}
