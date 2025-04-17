using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class AccessibilityFeature
{
    public int FeatureId { get; set; }

    public string FeatureName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PlaceFeature> PlaceFeatures { get; set; } = new List<PlaceFeature>();
}
