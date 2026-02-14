using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FinanceTrackerApp.Data;

public class FinanceDbContext : DbContext  
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Budget -> Category relationship
        modelBuilder.Entity<Budget>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Add indexes for performance
        modelBuilder.Entity<Budget>()
            .HasIndex(b => new { b.UserId, b.Year, b.Month });

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.UserId);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => new { t.UserId, t.Date });
    }
}