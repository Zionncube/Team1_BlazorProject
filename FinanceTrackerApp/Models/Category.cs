namespace FinanceTrackerApp.Models;

public class Category
{
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user";
    public string Name { get; set; } = "General";
    public string Color { get; set; } = "#2BEF83";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
