using BankingPortal.Application.DTOs;
using BankingPortal.Application.Interfaces;
using BankingPortal.Domain.Entities;
using BankingPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankingPortalDbContext _context;

        public CustomerService(BankingPortalDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerResponseDto> RegisterCustomerAsync(long userId, CustomerRegisterDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ApplicationException("User not found.");

            // Timestamp based unique Customer Number (e.g. CUST20260728223015)
            var customerNumber = $"CUST{DateTime.UtcNow:yyyyMMddHHmmss}";

            var customer = new Customer
            {
                CustomerNumber = customerNumber,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Email = dto.Email,
                Phone = dto.Phone,
                Status = "Pending_Approval",
                CreatedDate = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Link CustomerId back to User
            user.CustomerId = customer.CustomerId;

            // Create KYC entry
            var kyc = new CustomerKyc
            {
                CustomerId = customer.CustomerId,
                AadharNumber = dto.AadharNumber,
                PanNumber = dto.PanNumber,
                PassportNumber = dto.PassportNumber,
                VerificationStatus = "Pending",
                CreatedDate = DateTime.UtcNow
            };

            _context.CustomerKycs.Add(kyc);
            await _context.SaveChangesAsync();

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                CustomerNumber = customer.CustomerNumber,
                FullName = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email ?? "",
                Phone = customer.Phone ?? "",
                Status = customer.Status,
                CreatedDate = customer.CreatedDate,
                VerificationStatus = kyc.VerificationStatus
            };
        }

        public async Task<List<CustomerResponseDto>> GetPendingApprovalsAsync()
        {
            return await _context.Customers
                .Where(c => c.Status == "Pending_Approval")
                .Select(c => new CustomerResponseDto
                {
                    CustomerId = c.CustomerId,
                    CustomerNumber = c.CustomerNumber,
                    FullName = $"{c.FirstName} {c.LastName}",
                    Email = c.Email ?? "",
                    Phone = c.Phone ?? "",
                    Status = c.Status,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveCustomerAsync(long customerId, long adminUserId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || customer.Status != "Pending_Approval") return false;

            customer.Status = "Active";
            customer.ApprovedBy = adminUserId;
            customer.ApprovedDate = DateTime.UtcNow;

            // Approve KYC as well
            var kyc = await _context.CustomerKycs.FirstOrDefaultAsync(k => k.CustomerId == customerId);
            if (kyc != null)
            {
                kyc.VerificationStatus = "Verified";
                kyc.VerifiedDate = DateTime.UtcNow;
                kyc.UpdatedDate = DateTime.UtcNow;
            }

            // Timestamp based unique Account Number (e.g. ACC20260728223015)
            var accountNumber = $"ACC{DateTime.UtcNow:yyyyMMddHHmmss}";
            var account = new Account
            {
                CustomerId = customer.CustomerId,
                AccountNumber = accountNumber,
                AccountTypeId = 1, // Savings Account
                Balance = 1000.00m, // Initial Welcome Balance
                Status = "Active",
                BranchId = 1,
                CreatedBy = adminUserId,
                OpenedDate = DateTime.UtcNow
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCustomerAsync(long customerId, string reason)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null || customer.Status != "Pending_Approval") return false;

            customer.Status = "Rejected";
            customer.RejectionReason = reason;
            customer.UpdatedDate = DateTime.UtcNow;

            var kyc = await _context.CustomerKycs.FirstOrDefaultAsync(k => k.CustomerId == customerId);
            if (kyc != null)
            {
                kyc.VerificationStatus = "Rejected";
                kyc.UpdatedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}