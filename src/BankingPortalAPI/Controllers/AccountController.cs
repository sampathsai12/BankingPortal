using System.Security.Claims;
using BankingPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingPortalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires authenticated JWT token
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerService _customerService;

    public AccountController(IAccountRepository accountRepository, ICustomerService customerService)
    {
        _accountRepository = accountRepository;
        _customerService = customerService;
    }

    /// <summary>
    /// Gets all bank accounts linked to the logged-in customer.
    /// </summary>
    [HttpGet("my-accounts")]
    public async Task<IActionResult> GetMyAccounts()
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Fetch customer linked to this user
        var customer = await _customerService.GetCustomerByUserIdAsync(userId);
        if (customer == null)
        {
            return NotFound(new { message = "Customer profile not found for this user." });
        }

        var accounts = await _accountRepository.GetAccountsByCustomerIdAsync(customer.CustomerId);

        var result = accounts.Select(a => new
        {
            a.AccountId,
            a.AccountNumber,
            a.Balance,
            a.Status,
            a.OpenedDate
        });

        return Ok(result);
    }

    /// <summary>
    /// Gets specific account balance by account number.
    /// </summary>
    [HttpGet("balance/{accountNumber}")]
    public async Task<IActionResult> GetBalance(string accountNumber)
    {
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
        if (account == null)
        {
            return NotFound(new { message = "Account not found." });
        }

        return Ok(new
        {
            account.AccountNumber,
            account.Balance,
            account.Status
        });
    }
}