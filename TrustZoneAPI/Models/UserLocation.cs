using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class UserLocation
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
