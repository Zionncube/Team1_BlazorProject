using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services
{
    public class TransactionService
    {
        private readonly FinanceDbContext _db;

        public TransactionService(FinanceDbContext db)
        {
            _db = db;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(string userId)
        {
            return await _db.Transactions
                            .Where(t => t.UserId == userId)
                            .OrderByDescending(t => t.Date)
                            .ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            var tx = await _db.Transactions.FindAsync(transactionId);
            if (tx != null)
            {
                _db.Transactions.Remove(tx);
                await _db.SaveChangesAsync();
            }
        }
    }
}
