using BankingPortal.Application.DTOs;
using BankingPortal.Application.Interfaces;
using BankingPortal.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict access to Admins only
    public class AdminController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public AdminController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("pending-customers")]
        public async Task<IActionResult> GetPendingCustomers()
        {
            var pending = await _customerService.GetPendingApprovalsAsync();
            return Ok(pending);
        }

        [HttpPost("approve-customer/{customerId}")]
        public async Task<IActionResult> ApproveCustomer(long customerId)
        {
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            long adminUserId = string.IsNullOrEmpty(adminIdClaim) ? 1 : long.Parse(adminIdClaim);

            var success = await _customerService.ApproveCustomerAsync(customerId, adminUserId);
            if (!success) return BadRequest(new { message = "Unable to approve customer or customer not in pending state." });

            return Ok(new { message = "Customer approved and primary bank account created successfully!" });
        }

        [HttpPost("reject-customer/{customerId}")]
        public async Task<IActionResult> RejectCustomer(long customerId, [FromBody] RejectCustomerDto dto)
        {
            var success = await _customerService.RejectCustomerAsync(customerId, dto.Reason);
            if (!success) return BadRequest(new { message = "Unable to reject customer or customer not in pending state." });

            return Ok(new { message = "Customer profile rejected successfully." });
        }
    }
}