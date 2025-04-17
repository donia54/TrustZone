using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
