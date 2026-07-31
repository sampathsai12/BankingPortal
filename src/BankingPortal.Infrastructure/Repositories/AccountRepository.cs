using BankingPortal.Application.Interfaces;
using BankingPortal.Domain.Entities;
using BankingPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(BankingPortalDbContext context) : base(context) { }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task<Account?> GetByCustomerIdAsync(long customerId)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.CustomerId == customerId);
    }

    public async Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(long customerId)
    {
        return await _dbSet.Where(a => a.CustomerId == customerId).ToListAsync();
    }
}