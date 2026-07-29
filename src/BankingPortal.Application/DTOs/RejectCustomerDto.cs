using System.ComponentModel.DataAnnotations;

namespace BankingPortal.Application.DTOs
{
    public class RejectCustomerDto
    {
        [Required(ErrorMessage = "Rejection reason is required")]
        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;
    }
}