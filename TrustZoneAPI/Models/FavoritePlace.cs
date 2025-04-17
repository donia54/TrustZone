using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class FavoritePlace
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int BranchId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
