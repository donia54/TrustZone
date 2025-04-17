namespace TrustZoneAPI.DTOs.Places
{
    public class PlaceWithBranchesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<BranchLightDTO> Branches { get; set; } = new();
    }
}
