using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class BranchBooking
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public string UserId { get; set; } = null!;

    public int BookingTypeId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? SpecialRequirements { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BookingType BookingType { get; set; } = null!;

    public virtual Branch Branch { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
