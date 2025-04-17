using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
