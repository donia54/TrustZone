using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class AichatMessage
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string MessageText { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public string? ContextData { get; set; }

    public int? ConversationId { get; set; }

    public virtual AichatConversation? Conversation { get; set; }

    public virtual User User { get; set; } = null!;
}
