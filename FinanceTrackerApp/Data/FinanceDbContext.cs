using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Data;

/// <summary>
/// EF Core database context for the Finance Tracker application.
/// Centralizes DbSets and entity configuration for SQLite persistence.
/// </summary>
public class FinanceDbContext : DbContext  
{
    /// <summary>
    /// Creates the context with externally provided options.
    /// </summary>
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }

    /// <summary>Stored user transactions.</summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();
    /// <summary>Registered local users.</summary>
    public DbSet<User> Users => Set<User>();
    /// <summary>User-defined transaction categories.</summary>
    public DbSet<Category> Categories => Set<Category>();
    /// <summary>User savings goals.</summary>
    public DbSet<SavingsGoal> SavingsGoals => Set<SavingsGoal>();
    /// <summary>Contributions made toward savings goals.</summary>
    public DbSet<GoalContribution> GoalContributions => Set<GoalContribution>();

    /// <summary>
    /// Configures keys and indexes for core entities.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasKey(c => c.CategoryId);

        modelBuilder.Entity<SavingsGoal>()
            .HasKey(g => g.GoalId);

        modelBuilder.Entity<GoalContribution>()
            .HasKey(c => c.ContributionId);

        modelBuilder.Entity<GoalContribution>()
            .HasIndex(c => new { c.UserId, c.GoalId });

        modelBuilder.Entity<SavingsGoal>()
            .HasIndex(g => g.UserId);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.UserId);
    }
}
