using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Places
{
    public class PlaceSearchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreatePlaceDTO
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Details { get; set; }
        public List<int>? FeatureIds { get; set; }
    }
    public class PlaceDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Url]
        public string? Website { get; set; }

        [Range(-90, 90)]
        public decimal? Latitude { get; set; }

        [Range(-180, 180)]
        public decimal? Longitude { get; set; }

        public string? Details { get; set; }
        public List<int>? FeatureIds { get; set; }
    }

    
}
