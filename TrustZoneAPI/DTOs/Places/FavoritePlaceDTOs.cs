namespace TrustZoneAPI.DTOs.Places
{
    public class FavoritePlaceDto
    {
        public int Id { get; set; }
       
       public  BranchLightDTO Branch { get; set; } = null!;

        public BranchPhotoDto BranchPhotoDto { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateFavoritePlaceDto
    {
      //  public string UserId { get; set; } = null!;
        public int BranchId { get; set; }
    }
}
