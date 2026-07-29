using System.Security.Claims;
using BankingPortal.Application.DTOs;
using BankingPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register-profile")]
        public async Task<IActionResult> RegisterProfile([FromBody] CustomerRegisterDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = long.Parse(userIdClaim);
            var result = await _customerService.RegisterCustomerAsync(userId, dto);
            return Ok(result);
        }
    }
}