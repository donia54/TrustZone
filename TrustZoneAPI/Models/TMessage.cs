using System;
using System.Collections.Generic;

namespace TrustZoneAPI.Models;

public partial class TMessage
{
    public int Id { get; set; }

    public int ConversationId { get; set; }

    public string SenderId { get; set; } = null!;

    public string MessageText { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
