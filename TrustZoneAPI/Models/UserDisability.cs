using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class UserDisability
{
    public int Id { get; set; }

    public int DisabilityTypeId { get; set; }

    public string UserId { get; set; } = null!;

    public virtual DisabilityType DisabilityType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
