using BankingPortal.Application.DTOs;

namespace BankingPortal.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDto> DepositAsync(long userId, DepositRequestDto request);
    Task<TransactionResponseDto> WithdrawAsync(long userId, WithdrawRequestDto request);
    Task<TransactionResponseDto> TransferAsync(long userId, TransferRequestDto request);
    Task<IEnumerable<TransactionResponseDto>> GetTransactionHistoryAsync(long userId, string accountNumber);
}