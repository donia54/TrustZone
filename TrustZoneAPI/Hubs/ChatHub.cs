using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.SignalR;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Hubs;

public class chatHub : Hub
{
    // Concurrent dictionary to store the last seen time for each user
    private static readonly ConcurrentDictionary<string, DateTime> UserLastSeen = new();


    private readonly IUserService _userService;
    private readonly IConnectionService _connectionService;

    public chatHub(IUserService userService, IConnectionService connectionManager)
    {
        _userService = userService;
        _connectionService = connectionManager;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        string token = httpContext.Request.Query["access_token"].ToString();

        if (!string.IsNullOrWhiteSpace(token))
        {

            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Uid")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                _connectionService.AddConnection(userId, Context.ConnectionId);
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
            _connectionService.RemoveConnection(userId);
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
