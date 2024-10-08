using Shared.Models;

namespace Shared.Repositories
{
    public interface IUserServerRepository : IBaseUserRepository
    {
        Task AddUser(User user);
        Task<User> Authenticate(string username, string password);
    }
}
