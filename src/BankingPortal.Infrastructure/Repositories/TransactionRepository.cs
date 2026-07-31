using BankingPortal.Application.Interfaces;
using BankingPortal.Domain.Entities;
using BankingPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Repositories;

public class TransactionRepository : Repository<TransactionLedger>, ITransactionRepository
{
    public TransactionRepository(BankingPortalDbContext context) : base(context) { }

    public async Task<IEnumerable<TransactionLedger>> GetTransactionHistoryByAccountIdAsync(long accountId)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }
}