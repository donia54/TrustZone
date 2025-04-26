namespace TrustZoneAPI.DTOs.Users
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverPicture { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserLightDTO
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }
    }

}
