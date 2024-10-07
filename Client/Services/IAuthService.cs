using Shared.Models;

namespace Client.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginModel);
        Task Logout();
        Task Register(User registerModel);
    }
}
