namespace TrustZoneAPI.DTOs
{
    public class AuthDTO
    {
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }

    }
}
