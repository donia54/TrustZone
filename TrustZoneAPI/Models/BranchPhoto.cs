using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class BranchPhoto
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public string PhotoUrl { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public string? UploadedByUserId { get; set; }

    public bool IsFeatured { get; set; }

    public string? Caption { get; set; }

    public bool IsApproved { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User? UploadedByUser { get; set; }
}
