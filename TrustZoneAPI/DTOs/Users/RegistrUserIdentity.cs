using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.User
{
    public class RegistrUserIdentity
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        public required string UserName { get; set; }

        [Required]
        public int Age { get; set; }

        public int DisabilityTypeId { get; set; }
    }
}
