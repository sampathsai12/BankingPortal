using BankingPortal.Application.DTOs;
using FluentValidation;

namespace BankingPortal.Application.Validators;

public class DepositRequestValidator : AbstractValidator<DepositRequestDto>
{
    public DepositRequestValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Deposit amount must be greater than zero.");
    }
}

public class WithdrawRequestValidator : AbstractValidator<WithdrawRequestDto>
{
    public WithdrawRequestValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Withdrawal amount must be greater than zero.");
    }
}

public class TransferRequestValidator : AbstractValidator<TransferRequestDto>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.SourceAccountNumber).NotEmpty().WithMessage("Source account number is required.");
        RuleFor(x => x.TargetAccountNumber).NotEmpty().WithMessage("Target account number is required.");
        RuleFor(x => x.SourceAccountNumber)
            .NotEqual(x => x.TargetAccountNumber)
            .WithMessage("Source and Target accounts cannot be the same.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transfer amount must be greater than zero.");
    }
}