using Microsoft.AspNetCore.SignalR;
using TrustZoneAPI.DTOs.Places;

namespace TrustZoneAPI.Services
{
    public class SearchHub : Hub
    {
        public async Task SendSearchResults(List<PlaceSearchDTO> results)
        {
            await Clients.All.SendAsync("ReceiveSearchResults", results);
        }
    }
}
