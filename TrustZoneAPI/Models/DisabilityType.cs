using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class DisabilityType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserDisability> UserDisabilities { get; set; } = new List<UserDisability>();
}
