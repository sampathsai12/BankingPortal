namespace BankingPortal.Application.DTOs;

public record DepositRequestDto(string AccountNumber, decimal Amount, string Description);

public record WithdrawRequestDto(string AccountNumber, decimal Amount, string Description);

public record TransferRequestDto(string SourceAccountNumber, string TargetAccountNumber, decimal Amount, string Description);

public record TransactionResponseDto(
    long TransactionId,
    string AccountNumber,
    string TransactionType,
    decimal Amount,
    decimal BalanceAfterTransaction,
    string Status,
    string ReferenceNumber,
    DateTime TransactionDate,
    string Description
);