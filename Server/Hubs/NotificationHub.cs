using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string userName, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
        public async Task NotifyRoleChange(string userId)
        {
            await Clients.All.SendAsync("RoleChanged", userId);
        }
    }
}
