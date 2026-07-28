using BankingPortal.Application.DTOs;
using BankingPortal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result) return BadRequest(new { message = "Username already exists." });

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null) return Unauthorized(new { message = "Invalid username or password." });

            return Ok(new { token });
        }
    }
}