using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Hubs;

public class ChatHub : Hub
{
    // Concurrent dictionary to store the last seen time for each user
    private static readonly ConcurrentDictionary<string, DateTime> UserLastSeen = new();
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();


    private readonly IUserService _userService;
    public ChatHub(IUserService userService)
    {
        _userService = userService;
    }
    // send message to a specific user
    public async Task SendMessage(string receiverId, string senderId, string message)
    {
        if (UserConnections.TryGetValue(receiverId, out var receiverConnectionId))
        {
            Console.WriteLine($"SendMessage called with receiverId={receiverId}, senderId={senderId}");

            if (receiverConnectionId != null)
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message);
        }
        else
        {
            // إذا لم يكن المستخدم متصلًا
        //    await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }
    }
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var token = httpContext.Request.Query["access_token"].ToString();

        if (!string.IsNullOrWhiteSpace(token))
        {

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Uid")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
            }
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
public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
