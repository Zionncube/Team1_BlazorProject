using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

public interface ITransactionsStore
{
    Task<List<Transaction>> GetTransactionsAsync(string userId, CancellationToken ct = default);
    Task AddTransactionAsync(string userId, Transaction transaction, CancellationToken ct = default);
}
