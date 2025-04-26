namespace TrustZoneAPI.DTOs.Disabilities
{
    public class DisabilityTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateDisabilityTypeDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
