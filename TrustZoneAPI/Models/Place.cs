using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class Place
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<PlaceFeature> PlaceFeatures { get; set; } = new List<PlaceFeature>();
}
