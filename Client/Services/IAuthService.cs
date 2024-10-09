using Shared.Models;

namespace Client.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Logout();
        Task SaveToken(string token);
        Task RemoveToken();
    }
}
