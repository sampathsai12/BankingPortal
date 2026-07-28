using System;
using System.Collections.Generic;
using System.Text;

namespace BankingPortal.Application.DTOs
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
