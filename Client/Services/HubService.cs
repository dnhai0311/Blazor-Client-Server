using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Services
{
    public class HubService : IAsyncDisposable
    {
        private HubConnection? HubConnection;
        public List<string> messages = new();

        public event Action<string>? OnMessageReceived;
        public event Action<string>? OnRoleChanged;


        public async Task Initialize()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7103/hubs")
                .Build();

            HubConnection.On<string, string>("ReceiveMessage", (userName, message) =>
            {
                var encodedMsg = $"{userName}: {message}";
                messages.Add(encodedMsg);
                OnMessageReceived?.Invoke(encodedMsg);
            });

            HubConnection.On<string>("RoleChanged", (userId) =>
            {
                OnRoleChanged?.Invoke(userId);
            });

            await HubConnection.StartAsync();
        }

        public async Task SendMessage(string userName, string message)
        {
            if (HubConnection is not null && !string.IsNullOrWhiteSpace(message))
            {
                await HubConnection.SendAsync("SendMessage", userName, message);
            }
        }

        public async Task ChangeRole(string userId)
        {
            if (HubConnection is not null && !string.IsNullOrWhiteSpace(userId))
            {
                await HubConnection.SendAsync("NotifyRoleChange", userId);
            }
        }

        public bool IsConnected => HubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }

        public List<string> GetMessages() => messages;
    }
}
