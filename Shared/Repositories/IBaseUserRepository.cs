﻿using Shared.Models;

namespace Shared.Repositories
{
    public interface IBaseUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
        Task ChangePassword(int userId, ChangePasswordRequest changePasswordRequest);
        Task UpdateUserStatus(int userId, bool isActive);
    }
}