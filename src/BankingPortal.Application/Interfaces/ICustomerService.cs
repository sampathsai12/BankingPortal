using BankingPortal.Application.DTOs;
using BankingPortal.Domain.Entities;

namespace BankingPortal.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> RegisterCustomerAsync(long userId, CustomerRegisterDto dto);
        Task<List<CustomerResponseDto>> GetPendingApprovalsAsync();
        Task<bool> ApproveCustomerAsync(long customerId, long adminUserId);
        Task<bool> RejectCustomerAsync(long customerId, string reason);
        Task<Customer?> GetCustomerByUserIdAsync(long userId);
    }
}