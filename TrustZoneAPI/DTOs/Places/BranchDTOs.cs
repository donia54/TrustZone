using System.Diagnostics.CodeAnalysis;
using static TrustZoneAPI.DTOs.Places.ReviewsDTOs;

namespace TrustZoneAPI.DTOs.Places
{
    public class BranchLightDTO
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
    }
    public class BranchDTO
    {
        public int Id { get; set; }

        //public int PlaceId { get; set; }
        public string Address { get; set; } = null!;
        public string? Website { get; set; }
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<BranchOpeningHourDTO>? OpiningHours { get; set; }

        public PlaceBranchDTO Place { get; set; } = null!;
   
    }

    
    public class CreateBranchDTO
    {
        public int PlaceId { get; set; }
        public string Address { get; set; } = null!;
        public string? Website { get; set; }
        public string? Phone { get; set; }
    }
    public class UpdateBranchDTO
    {
        public string Address { get; set; } = null!;
        public string? Website { get; set; }
        public string? Phone { get; set; }  
    }

    public class BranchOpeningHourDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeOnly? OpeningTime { get; set; } = null!;
        public TimeOnly? ClosingTime { get; set; } = null!;

        public bool IsClosed { get; set; }
    }

    public class CreateBranchOpeningHourDTO
    {
        public int BranchId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
        public bool IsClosed { get; set; }
    }

    public class UpdateBranchOpeningHourDTO
    {
        public byte DayOfWeek { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
        public bool IsClosed { get; set; }
        public string? SpecialNotes { get; set; }
    }
}
