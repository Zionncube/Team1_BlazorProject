namespace FinanceTrackerApp.Models;

/// <summary>
/// Local authentication user stored in SQLite.
/// </summary>
public class User
{
    /// <summary>Stable user identifier used by related records.</summary>
    public string UserId { get; set; } = Guid.NewGuid().ToString();
    /// <summary>Normalized login email.</summary>
    public string Email { get; set; } = "";
    /// <summary>PBKDF2-hashed password and salt.</summary>
    public string PasswordHash { get; set; } = "";
    /// <summary>UTC creation timestamp.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
