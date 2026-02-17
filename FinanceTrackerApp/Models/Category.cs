namespace FinanceTrackerApp.Models;

/// <summary>
/// Represents a transaction category such as Food, Transport, or Salary.
/// Categories help organize financial records.
/// </summary>
public class Category
{
    /// <summary>Unique identifier for the category</summary>
    public Guid CategoryId { get; set; } = Guid.NewGuid();

    /// <summary>User that owns the category</summary>
    public string UserId { get; set; } = "demo-user";

    /// <summary>Name displayed to the user</summary>
    public string Name { get; set; } = "General";

    /// <summary>Color used in UI and charts</summary>
    public string Color { get; set; } = "#2BEF83";
}