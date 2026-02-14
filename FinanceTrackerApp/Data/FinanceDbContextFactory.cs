using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FinanceTrackerApp.Data
{
    public class FinanceDbContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
    {
        public FinanceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder.UseSqlite("Data Source=FinanceTracker.db")
                .ConfigureWarnings(w =>
                {
                    w.Ignore(RelationalEventId.PendingModelChangesWarning);
                    w.Default(WarningBehavior.Log);
                });

            return new FinanceDbContext(optionsBuilder.Options);
        }
    }
}
