using System;
using System.ComponentModel.DataAnnotations;

public class ChatMessage
{
    public int Id { get; set; }

    public int ListingId { get; set; }

    [Required]
    public string SenderUserId { get; set; } = "";

    [Required]
    public string SenderDisplayName { get; set; } = "";

    [Required]
    public string Text { get; set; } = "";

    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
}
