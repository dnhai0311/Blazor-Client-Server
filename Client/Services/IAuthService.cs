using Shared.Models;

namespace Client.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        void Logout();
        Task SaveToken(string token);
        Task RemoveToken();
    }
}
