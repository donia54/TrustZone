using System.ComponentModel.DataAnnotations;
using TrustZoneAPI.DTOs.Disabilities;

namespace TrustZoneAPI.DTOs.Users
{
    public class UserPictureDTO
    {
        public string FileName { get; set; } 
        public string Url { get; set; }      
        public string Type { get; set; }     // "profile" Or "cover"
    }

    public class UpdatePasswordDTO
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }

    public class UserProfileDTO
    {
        [Display(Name = "User ID")]
        public string Id { get; set; } = null!;

        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureUrl { get; set; }

        [Display(Name = "Cover Picture URL")]
        public string? CoverPictureUrl { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        public List<DisabilityTypeDTO> DisabilityTypes { get; set; } = new List<DisabilityTypeDTO>();
    }

    public class UpdateUserProfileDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }


        [Required]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int? Age { get; set; }
    }



}
