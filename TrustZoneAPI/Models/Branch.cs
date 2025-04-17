using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class Branch
{
    public int Id { get; set; }

    public int PlaceId { get; set; }

    public string Address { get; set; } = null!;

    public string? Website { get; set; }

    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AccessibilityEvent> AccessibilityEvents { get; set; } = new List<AccessibilityEvent>();

    public virtual ICollection<BranchBooking> BranchBookings { get; set; } = new List<BranchBooking>();

    public virtual ICollection<BranchOpeningHour> BranchOpeningHours { get; set; } = new List<BranchOpeningHour>();

    public virtual ICollection<BranchPhoto> BranchPhotos { get; set; } = new List<BranchPhoto>();

    public virtual ICollection<FavoritePlace> FavoritePlaces { get; set; } = new List<FavoritePlace>();

    public virtual Place Place { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<SavedPlace> SavedPlaces { get; set; } = new List<SavedPlace>();
}
