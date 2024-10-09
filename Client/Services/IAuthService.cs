using Shared.Models;

namespace Client.Services
{
    public interface IAuthService
    {
        Task<RegisterResult> Register(RegisterRequest registerRequest);
        Task<LoginResult> Login(LoginRequest loginRequest);
        Task Logout();
        Task SaveToken(string token);
        Task RemoveToken();
    }
}
