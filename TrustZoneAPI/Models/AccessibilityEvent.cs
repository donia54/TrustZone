using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class AccessibilityEvent
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public string EventName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? SpecialFeaturesAvailable { get; set; }

    public string? OrganizerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User? Organizer { get; set; }
}
