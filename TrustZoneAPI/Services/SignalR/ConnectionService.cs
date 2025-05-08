using System.Collections.Concurrent;

namespace TrustZoneAPI.Services.SignalR;
public interface IConnectionService
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId);
    string? GetConnectionId(string userId);
}
public class ConnectionService : IConnectionService
{
    private readonly ConcurrentDictionary<string, string> _connections = new();

    public void AddConnection(string userId, string connectionId)
    {
        _connections[userId] = connectionId;
    }

    public void RemoveConnection(string userId)
    {
        _connections.TryRemove(userId, out _);
    }

    public string? GetConnectionId(string userId)
    {
        _connections.TryGetValue(userId, out var connectionId);
        return connectionId;
    }
}
