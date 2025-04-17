using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class User : IdentityUser
{
  //  public string Id { get; set; } = null!;

    public int Age { get; set; }

    public string? ProfilePicture { get; set; }

    public string? CoverPicture { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AccessibilityEvent> AccessibilityEvents { get; set; } = new List<AccessibilityEvent>();

    public virtual ICollection<AichatConversation> AichatConversations { get; set; } = new List<AichatConversation>();

    public virtual ICollection<AichatMessage> AichatMessages { get; set; } = new List<AichatMessage>();

    //public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    //public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    //public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<BranchBooking> BranchBookings { get; set; } = new List<BranchBooking>();

    public virtual ICollection<BranchPhoto> BranchPhotos { get; set; } = new List<BranchPhoto>();

    public virtual ICollection<Conversation> ConversationUser1s { get; set; } = new List<Conversation>();

    public virtual ICollection<Conversation> ConversationUser2s { get; set; } = new List<Conversation>();

    public virtual ICollection<FavoritePlace> FavoritePlaces { get; set; } = new List<FavoritePlace>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<SavedPlace> SavedPlaces { get; set; } = new List<SavedPlace>();

    public virtual ICollection<TMessage> TMessages { get; set; } = new List<TMessage>();

    public virtual ICollection<UserDisability> UserDisabilities { get; set; } = new List<UserDisability>();

   // public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
