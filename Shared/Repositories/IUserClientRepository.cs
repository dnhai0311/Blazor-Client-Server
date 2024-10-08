using Shared.Models;

namespace Shared.Repositories
{
    public interface IUserClientRepository : IBaseUserRepository
    {
        Task AddUser(RegisterRequest registerRequest);
    }
}
