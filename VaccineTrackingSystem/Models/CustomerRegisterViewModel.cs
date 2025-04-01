using System.ComponentModel.DataAnnotations;

namespace VaccineTrackingSystem.Models
{
    public class CustomerRegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        // Thông tin cho Child (ít nhất 1 con)
        [Required]
        public string ChildFullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime ChildDateOfBirth { get; set; }

        [Required]
        public byte ChildGender { get; set; } // 0: Male, 1: Female, 2: Other

        public string? ChildAdditionalInfo { get; set; }
    }
}
