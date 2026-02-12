using FinanceTrackerApp.Models;

namespace FinanceTrackerApp.Services;

public class InMemoryTransactionsStore : ITransactionsStore
{
    private readonly Dictionary<string, List<Transaction>> _store = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();

    public Task<List<Transaction>> GetTransactionsAsync(string userId, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(userId, out var transactions))
            {
                transactions = BuildSeed(userId);
                _store[userId] = transactions;
            }

            return Task.FromResult(transactions
                .OrderByDescending(t => t.Date)
                .ToList());
        }
    }

    public Task AddTransactionAsync(string userId, Transaction transaction, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_store.TryGetValue(userId, out var transactions))
            {
                transactions = BuildSeed(userId);
                _store[userId] = transactions;
            }

            transaction.TransactionId = transaction.TransactionId == Guid.Empty ? Guid.NewGuid() : transaction.TransactionId;
            transaction.UserId = userId;
            transaction.Description = string.IsNullOrWhiteSpace(transaction.Description) ? "Transaction" : transaction.Description.Trim();
            transaction.Category = string.IsNullOrWhiteSpace(transaction.Category) ? "General" : transaction.Category.Trim();
            transaction.Type = string.Equals(transaction.Type, "income", StringComparison.OrdinalIgnoreCase) ? "income" : "expense";
            transactions.Add(transaction);
        }

        return Task.CompletedTask;
    }

    private static List<Transaction> BuildSeed(string userId)
    {
        return new List<Transaction>
        {
            new()
            {
                UserId = userId,
                Amount = 4800m,
                Category = "Salary",
                Type = "income",
                Date = DateTime.Today.AddDays(-3),
                Description = "Monthly salary"
            },
            new()
            {
                UserId = userId,
                Amount = 420m,
                Category = "Food",
                Type = "expense",
                Date = DateTime.Today.AddDays(-1),
                Description = "Groceries"
            },
            new()
            {
                UserId = userId,
                Amount = 95m,
                Category = "Transport",
                Type = "expense",
                Date = DateTime.Today.AddDays(-2),
                Description = "Taxi"
            }
        };
    }
}
