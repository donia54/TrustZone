namespace TrustZoneAPI.DTOs.Places
{
    public class BranchPhotoDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public DateTime UploadDate { get; set; }
        public string? UploadedByUserId { get; set; }
       // public bool IsFeatured { get; set; }
       // public string? Caption { get; set; }
       // public bool IsApproved { get; set; }
    }

    public class CreateBranchPhotoDto
    {
        public int BranchId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        //public string? UploadedByUserId { get; set; }
        //public bool IsFeatured { get; set; }
        //public string? Caption { get; set; }
        //public bool IsApproved { get; set; }
    }
}
