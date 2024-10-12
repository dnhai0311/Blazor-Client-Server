using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task NotifyRoleChange(int userId)
        {
            await Clients.All.SendAsync("RoleChanged");
        }
    }
}
