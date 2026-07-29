using System.ComponentModel.DataAnnotations;

namespace BankingPortal.Domain.Entities
{
    public class CustomerKyc
    {
        [Key]
        public long KycId { get; set; }
        public long CustomerId { get; set; }
        public string? AadharNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public DateTime? VerifiedDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        public Customer Customer { get; set; } = null!;
    }
}