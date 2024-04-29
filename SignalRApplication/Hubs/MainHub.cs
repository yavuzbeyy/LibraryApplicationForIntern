using Microsoft.AspNetCore.SignalR;

namespace SignalRApplication.Hubs
{
    public class MainHub : Hub
    {
        public async Task BroadcastMessageToAllClient(string message) 
        {
            await Clients.All.SendAsync("ReceiveMesasgesForAllClients",message);
        }
    }
}
