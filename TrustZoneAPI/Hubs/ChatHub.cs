using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Hubs;

public class ChatHub : Hub
{
    // Concurrent dictionary to store the last seen time for each user
    private static readonly ConcurrentDictionary<string, DateTime> UserLastSeen = new();
    private readonly IUserService _userService;
    public ChatHub(IUserService userService)
    {
        _userService = userService;
    }
    // send message to a specific user
    public async Task SendMessage(string receiverId, string senderId, string message)
    {
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
    }

    // when user connects, we can update the last seen time
    public override async Task OnConnectedAsync()
    {
        var userId = _userService.GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            // Update last seen
            UserLastSeen[userId] = DateTime.UtcNow;
        }
        await base.OnConnectedAsync();
    }

    // when user disconnects, we can update the last seen time
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _userService.GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            // Update last seen
            UserLastSeen[userId] = DateTime.UtcNow;
        }
        await base.OnDisconnectedAsync(exception);
    }

    // method to get the last seen time for a user
    public DateTime? GetLastSeen(string userId)
    {
        if (UserLastSeen.TryGetValue(userId, out var lastSeen))
        {
            return lastSeen;
        }
        return null;
    }
}
