using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class Conversation
{
    public int Id { get; set; }

    public string User1Id { get; set; } = null!;

    public string User2Id { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public virtual ICollection<TMessage> TMessages { get; set; } = new List<TMessage>();

    public virtual User User1 { get; set; } = null!;

    public virtual User User2 { get; set; } = null!;
}
