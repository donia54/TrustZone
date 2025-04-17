using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class BranchOpeningHour
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public byte DayOfWeek { get; set; }

    public TimeOnly? OpeningTime { get; set; }

    public TimeOnly? ClosingTime { get; set; }

    public bool IsClosed { get; set; }

    public string? SpecialNotes { get; set; }

    public virtual Branch Branch { get; set; } = null!;
}
