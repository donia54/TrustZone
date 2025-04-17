using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class BookingType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<BranchBooking> BranchBookings { get; set; } = new List<BranchBooking>();
}
