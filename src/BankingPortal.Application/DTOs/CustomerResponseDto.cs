namespace BankingPortal.Application.DTOs
{
    public class CustomerResponseDto
    {
        public long CustomerId { get; set; }
        public string CustomerNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? VerificationStatus { get; set; }
    }
}