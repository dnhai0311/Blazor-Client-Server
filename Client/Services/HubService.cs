using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Services
{
    public class HubService : IAsyncDisposable
    {
        private HubConnection? hubConnection;
        private List<string> messages = new();

        public event Action<string> OnMessageReceived;
        public event Action<string> OnRoleChanged;


        public async Task Initialize(string userName)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7103/hubs")
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                OnMessageReceived?.Invoke(encodedMsg);
            });

            hubConnection.On<string>("RoleChanged", (userId) =>
            {
                OnRoleChanged?.Invoke(userId);
            });

            await hubConnection.StartAsync();
        }

        public async Task SendMessage(string userName, string message)
        {
            if (hubConnection is not null && !string.IsNullOrWhiteSpace(message))
            {
                await hubConnection.SendAsync("SendMessage", userName, message);
            }
        }

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        public List<string> GetMessages() => messages;
    }
}
