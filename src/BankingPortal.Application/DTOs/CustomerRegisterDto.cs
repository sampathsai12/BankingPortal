using System.ComponentModel.DataAnnotations;

namespace BankingPortal.Application.DTOs
{
    public class CustomerRegisterDto
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar must be exactly 12 digits")]
        public string? AadharNumber { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN must be exactly 10 characters")]
        public string? PanNumber { get; set; }

        [MaxLength(20)]
        public string? PassportNumber { get; set; }
    }
}