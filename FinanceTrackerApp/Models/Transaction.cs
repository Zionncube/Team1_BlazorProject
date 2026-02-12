using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApp.Models;

public class Transaction
{
    [Key]
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = "demo-user"; // temporary for UI stage

    [Required(ErrorMessage = "Amount is required!")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0!")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Category is required!")]
    public string Category { get; set; } = "General";
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Description is required!")]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Type is required!")]
    [RegularExpression("income|expense", ErrorMessage = "Type must be income or expense.")]
    public string Type { get; set; } = "expense";
}
