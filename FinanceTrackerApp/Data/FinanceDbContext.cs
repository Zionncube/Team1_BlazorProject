using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Data;

public class FinanceDbContext : DbContext  
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
}