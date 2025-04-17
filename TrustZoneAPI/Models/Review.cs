using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class Review
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int BranchId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ContentUrl { get; set; }

    public bool? IsVerified { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
