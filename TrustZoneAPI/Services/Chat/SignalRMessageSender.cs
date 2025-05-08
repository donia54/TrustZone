using Microsoft.AspNetCore.SignalR;
using TrustZoneAPI.Hubs;
using TrustZoneAPI.Services.SignalR;

namespace TrustZoneAPI.Services.Chat;
public interface ISignalRMessageSender
{
    Task SendToClient(string receiverId, string senderId, object message);
}
public class SignalRMessageSender : ISignalRMessageSender
{
    private readonly IHubContext<chatHub> _hubContext;
    private readonly IConnectionService _connectionService;

    public SignalRMessageSender(IHubContext<chatHub> hubContext, IConnectionService connectionService)
    {
        _hubContext = hubContext;
        _connectionService = connectionService;
    }

    public async Task SendToClient(string receiverId, string senderId, object message)
    {
        var connectionId = _connectionService.GetConnectionId(receiverId);
        if (!string.IsNullOrEmpty(connectionId))
        {
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveMessage", new
                {
                    content = message,
                    userId = senderId
                });
        }
    }
}
