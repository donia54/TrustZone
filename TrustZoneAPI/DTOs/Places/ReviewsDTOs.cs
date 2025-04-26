using System.ComponentModel.DataAnnotations;
using TrustZoneAPI.DTOs.Users;

namespace TrustZoneAPI.DTOs.Places
{
    public class ReviewsDTOs
    {
        public class ReviewDto
        {
            public int Id { get; set; }
            public UserLightDTO user { get; set; } = null!;
            public int BranchId { get; set; }
            public int Rating { get; set; }
            public string? Comment { get; set; }
           // public string? ContentUrl { get; set; }
           // public bool? IsVerified { get; set; }
            public DateTime CreatedAt { get; set; }
        }


        public class CreateReviewDto
        {
            [Required(ErrorMessage = "BranchId is required")]
            public int BranchId { get; set; }

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
            public int Rating { get; set; }

            [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
            public string? Comment { get; set; }
          //  public string? ContentUrl { get; set; }
        }


        public class UpdateReviewDto
        {

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
            public int Rating { get; set; }

            [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
            public string? Comment { get; set; }

           // public string? ContentUrl { get; set; }

        }

    }
}
