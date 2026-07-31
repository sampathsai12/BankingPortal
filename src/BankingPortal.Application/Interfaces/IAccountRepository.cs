using BankingPortal.Domain.Entities;

namespace BankingPortal.Application.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<Account?> GetByCustomerIdAsync(long customerId);
    Task<IEnumerable<Account>> GetAccountsByCustomerIdAsync(long customerId);
}