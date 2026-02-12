using FinanceTrackerApp.Data;
using FinanceTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApp.Services;

public class TransactionService
{
    private readonly FinanceDbContext _db;

    public TransactionService(FinanceDbContext db)
    {
        _db = db;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _db.Transactions
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _db.Transactions.FindAsync(id);
    }

    public async Task AddAsync(Transaction transaction)
    {
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _db.Transactions.Update(transaction);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tx = await _db.Transactions.FindAsync(id);
        if (tx != null)
        {
            _db.Transactions.Remove(tx);
            await _db.SaveChangesAsync();
        }
    }
}
