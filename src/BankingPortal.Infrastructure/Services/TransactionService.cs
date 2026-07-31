using BankingPortal.Application.DTOs;
using BankingPortal.Application.Interfaces;
using BankingPortal.Domain.Entities;
using BankingPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly BankingPortalDbContext _context;

    // Transaction Type IDs: 1 = Deposit (Credit), 2 = Withdrawal (Debit), 3 = Transfer Out (Debit), 4 = Transfer In (Credit)
    private const int TYPE_DEPOSIT = 1;
    private const int TYPE_WITHDRAWAL = 2;
    private const int TYPE_TRANSFER_OUT = 3;
    private const int TYPE_TRANSFER_IN = 4;

    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        BankingPortalDbContext context)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _context = context;
    }

    public async Task<TransactionResponseDto> DepositAsync(long userId, DepositRequestDto request)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(request.AccountNumber)
            ?? throw new KeyNotFoundException("Account not found.");

        // Deposit increases balance -> Credit
        account.Balance += request.Amount;
        _accountRepository.Update(account);

        var transaction = new TransactionLedger
        {
            AccountId = account.AccountId,
            TransactionTypeId = TYPE_DEPOSIT,
            CreditAmount = request.Amount,
            DebitAmount = 0.00m,
            BalanceAfterTransaction = account.Balance,
            Status = "Completed",
            ReferenceNumber = $"TXN{DateTime.UtcNow:yyyyMMddHHmmssfff}",
            TransactionDate = DateTime.UtcNow,
            Narration = string.IsNullOrWhiteSpace(request.Description) ? "Cash Deposit" : request.Description
        };

        await _transactionRepository.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return MapToResponseDto(transaction, account.AccountNumber, "Deposit", request.Amount);
    }

    public async Task<TransactionResponseDto> WithdrawAsync(long userId, WithdrawRequestDto request)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(request.AccountNumber)
            ?? throw new KeyNotFoundException("Account not found.");

        if (account.Balance < request.Amount)
        {
            throw new InvalidOperationException("Insufficient funds for withdrawal.");
        }

        // Withdrawal decreases balance -> Debit
        account.Balance -= request.Amount;
        _accountRepository.Update(account);

        var transaction = new TransactionLedger
        {
            AccountId = account.AccountId,
            TransactionTypeId = TYPE_WITHDRAWAL,
            DebitAmount = request.Amount,
            CreditAmount = 0.00m,
            BalanceAfterTransaction = account.Balance,
            Status = "Completed",
            ReferenceNumber = $"TXN{DateTime.UtcNow:yyyyMMddHHmmssfff}",
            TransactionDate = DateTime.UtcNow,
            Narration = string.IsNullOrWhiteSpace(request.Description) ? "Cash Withdrawal" : request.Description
        };

        await _transactionRepository.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return MapToResponseDto(transaction, account.AccountNumber, "Withdrawal", request.Amount);
    }

    public async Task<TransactionResponseDto> TransferAsync(long userId, TransferRequestDto request)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var sourceAccount = await _accountRepository.GetByAccountNumberAsync(request.SourceAccountNumber)
                ?? throw new KeyNotFoundException("Source account not found.");

            var targetAccount = await _accountRepository.GetByAccountNumberAsync(request.TargetAccountNumber)
                ?? throw new KeyNotFoundException("Target account not found.");

            if (sourceAccount.Balance < request.Amount)
            {
                throw new InvalidOperationException("Insufficient funds for transfer.");
            }

            // Debit Source Account
            sourceAccount.Balance -= request.Amount;
            _accountRepository.Update(sourceAccount);

            // Credit Target Account
            targetAccount.Balance += request.Amount;
            _accountRepository.Update(targetAccount);

            var refNumber = $"TXN{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            // Ledger Entry for Source (Debit)
            var sourceLedger = new TransactionLedger
            {
                AccountId = sourceAccount.AccountId,
                TransactionTypeId = TYPE_TRANSFER_OUT,
                DebitAmount = request.Amount,
                CreditAmount = 0.00m,
                BalanceAfterTransaction = sourceAccount.Balance,
                Status = "Completed",
                ReferenceNumber = refNumber,
                TransactionDate = DateTime.UtcNow,
                Narration = $"Transfer to {targetAccount.AccountNumber}: {request.Description}"
            };

            // Ledger Entry for Target (Credit)
            var targetLedger = new TransactionLedger
            {
                AccountId = targetAccount.AccountId,
                TransactionTypeId = TYPE_TRANSFER_IN,
                CreditAmount = request.Amount,
                DebitAmount = 0.00m,
                BalanceAfterTransaction = targetAccount.Balance,
                Status = "Completed",
                ReferenceNumber = refNumber,
                TransactionDate = DateTime.UtcNow,
                Narration = $"Transfer from {sourceAccount.AccountNumber}: {request.Description}"
            };

            await _transactionRepository.AddAsync(sourceLedger);
            await _transactionRepository.AddAsync(targetLedger);

            await _context.SaveChangesAsync();
            await dbTransaction.CommitAsync();

            return MapToResponseDto(sourceLedger, sourceAccount.AccountNumber, "Transfer_Out", request.Amount);
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<TransactionResponseDto>> GetTransactionHistoryAsync(long userId, string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber)
            ?? throw new KeyNotFoundException("Account not found.");

        var history = await _transactionRepository.GetTransactionHistoryByAccountIdAsync(account.AccountId);

        return history.Select(t =>
        {
            var typeName = t.TransactionTypeId switch
            {
                TYPE_DEPOSIT => "Deposit",
                TYPE_WITHDRAWAL => "Withdrawal",
                TYPE_TRANSFER_OUT => "Transfer_Out",
                TYPE_TRANSFER_IN => "Transfer_In",
                _ => "General"
            };

            var amount = t.CreditAmount > 0 ? t.CreditAmount : t.DebitAmount;

            return MapToResponseDto(t, accountNumber, typeName, amount);
        });
    }

    private static TransactionResponseDto MapToResponseDto(
        TransactionLedger transaction,
        string accountNumber,
        string transactionType,
        decimal amount)
    {
        return new TransactionResponseDto(
            transaction.TransactionId,
            accountNumber,
            transactionType,
            amount,
            transaction.BalanceAfterTransaction,
            transaction.Status,
            transaction.ReferenceNumber,
            transaction.TransactionDate,
            transaction.Narration ?? string.Empty
        );
    }
}