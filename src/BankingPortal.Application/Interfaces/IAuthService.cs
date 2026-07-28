using System;
using System.Collections.Generic;
using System.Text;
using BankingPortal.Application.DTOs;

namespace BankingPortal.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto request);
        Task<string?> LoginAsync(LoginRequestDto request);
    }
}
