using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApp.Models;

public class Budget
{
    [Key]
    public Guid BudgetId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user";
    public Guid CategoryId { get; set; }
    public decimal MonthlyLimit { get; set; }
    public int Year { get; set; } = DateTime.Today.Year;
    public int Month { get; set; } = DateTime.Today.Month;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}
