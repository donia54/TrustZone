using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Hubs;

public class chatHub : Hub
{
    // Concurrent dictionary to store the last seen time for each user
    private static readonly ConcurrentDictionary<string, DateTime> UserLastSeen = new();
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();


    private readonly IUserService _userService;
    public chatHub(IUserService userService)
    {
        _userService = userService;
    }
    // send message to a specific user
    public async Task SendMessage(string receiverId, string message)
    {
        var senderId = Context.UserIdentifier;
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);

    }
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId}");
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
public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
