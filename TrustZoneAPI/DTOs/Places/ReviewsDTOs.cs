namespace TrustZoneAPI.DTOs.Places
{
    public class ReviewsDTOs
    {
        public class ReviewDto
        {
            public int Id { get; set; }
            public string UserName { get; set; } = string.Empty;
            public int Rating { get; set; }
            public string Comment { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
        }
    }
}
