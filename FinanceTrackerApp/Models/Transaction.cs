using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApp.Models;

/// <summary>
/// Represents one user transaction record.
/// </summary>
public class Transaction
{
    /// <summary>Unique identifier for the transaction.</summary>
    [Key]
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    /// <summary>User that owns this transaction.</summary>
    public string UserId { get; set; } = "";

    /// <summary>Transaction amount.</summary>
    [Required(ErrorMessage = "Amount is required!")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0!")]
    public decimal Amount { get; set; }

    /// <summary>Category label selected by the user.</summary>
    [Required(ErrorMessage = "Category is required!")]
    public string Category { get; set; } = "General";
    /// <summary>Date of the transaction.</summary>
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>Short description displayed in lists and reports.</summary>
    [Required(ErrorMessage = "Please enter a description")]
    public string Description { get; set; } = "";

    /// <summary>Transaction direction: income or expense.</summary>
    [Required(ErrorMessage = "Type is required!")]
    [RegularExpression("income|expense", ErrorMessage = "Type must be income or expense.")]
    public string Type { get; set; } = "expense";
}
