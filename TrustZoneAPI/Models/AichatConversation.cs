using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class AichatConversation
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime LastActivityAt { get; set; }

    public byte Status { get; set; }

    public string? ConversationTopic { get; set; }

    public string? Language { get; set; }

    public virtual ICollection<AichatMessage> AichatMessages { get; set; } = new List<AichatMessage>();

    public virtual User User { get; set; } = null!;
}
