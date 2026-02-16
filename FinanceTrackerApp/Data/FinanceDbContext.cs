using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Data;

public class FinanceDbContext : DbContext  
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SavingsGoal> SavingsGoals => Set<SavingsGoal>();
    public DbSet<GoalContribution> GoalContributions => Set<GoalContribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
