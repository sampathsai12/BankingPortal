using BankingPortal.Domain.Entities;

namespace BankingPortal.Application.Interfaces;

public interface ITransactionRepository : IRepository<TransactionLedger>
{
    Task<IEnumerable<TransactionLedger>> GetTransactionHistoryByAccountIdAsync(long accountId);
}