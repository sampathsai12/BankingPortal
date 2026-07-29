using BankingPortal.Application.DTOs;

namespace BankingPortal.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseDto> RegisterCustomerAsync(long userId, CustomerRegisterDto dto);
        Task<List<CustomerResponseDto>> GetPendingApprovalsAsync();
        Task<bool> ApproveCustomerAsync(long customerId, long adminUserId);
        Task<bool> RejectCustomerAsync(long customerId, string reason);
    }
}