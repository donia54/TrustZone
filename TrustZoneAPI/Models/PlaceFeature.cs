using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class PlaceFeature
{
    public int Id { get; set; }

    public int PlaceId { get; set; }

    public int FeatureId { get; set; }

    public virtual AccessibilityFeature Feature { get; set; } = null!;

    public virtual Place Place { get; set; } = null!;
}
