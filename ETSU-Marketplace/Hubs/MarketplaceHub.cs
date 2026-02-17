using ETSU_Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ETSU_Marketplace.Hubs
{
    [Authorize]
    public class MarketplaceHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public MarketplaceHub(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task JoinListing(int listingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"listing:{listingId}");
        }

        public async Task SendListingMessage(int listingId, string message)
        {
            message = (message ?? "").Trim();
            if (string.IsNullOrWhiteSpace(message)) return;

            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return;

            var displayName = Context.User?.Identity?.Name ?? "User";

            var chat = new ChatMessage
            {
                ListingId = listingId,
                SenderUserId = userId,
                SenderDisplayName = displayName,
                Text = message,
                SentAtUtc = DateTime.UtcNow
            };

            _db.ChatMessages.Add(chat);
            await _db.SaveChangesAsync();

            await Clients.Group($"listing:{listingId}")
                .SendAsync("ReceiveListingMessage", new
                {
                    sender = chat.SenderDisplayName,
                    text = chat.Text,
                    sentAtUtc = chat.SentAtUtc
                });
        }
    }
}
