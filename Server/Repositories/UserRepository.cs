using Microsoft.EntityFrameworkCore;
using Server.Models;
using Shared.Models;
using Shared.Repositories;
using System.Net;

namespace Server.Repositories
{
    public class UserRepository : IUserServerRepository
    {
        private readonly BookSalesContext bookSalesContext;

        public UserRepository(BookSalesContext context)
        {
            bookSalesContext = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await bookSalesContext.Users
                            .OrderBy(user => user.Id)
                            .Include(user => user.Role)
                            .ToListAsync();
            foreach (var user in users)
            {
                user.Password = null;
            }
            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await bookSalesContext.Users
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {id} không tìm thấy.");
            }
            user.Password = null;
            return user;
        }

        public async Task AddUser(User user)
        {
            var existingUser = await bookSalesContext.Users
                .FirstOrDefaultAsync(u => u.UserName == user.UserName || u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User với UserName hoặc Email này đã tồn tại.");
            }

            var role = await bookSalesContext.Roles.FindAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role với ID: {user.RoleId} không tìm thấy.");

            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            bookSalesContext.Users.Add(user);
            await bookSalesContext.SaveChangesAsync();
        }


        public async Task UpdateUser(User user)
        {
            var existingUser = await bookSalesContext.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User với ID: {user.Id} không tìm thấy.");
            }

            var otherUser = await bookSalesContext.Users
                .FirstOrDefaultAsync(u => (u.UserName == user.UserName || u.Email == user.Email) && u.Id != user.Id);
            if (otherUser != null)
            {
                throw new InvalidOperationException("User với UserName hoặc Email này đã tồn tại.");
            }

            var role = await bookSalesContext.Roles.FindAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role với ID: {user.RoleId} không tìm thấy.");
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            bookSalesContext.Entry(existingUser).CurrentValues.SetValues(user);

            await bookSalesContext.SaveChangesAsync();
        }



        public async Task DeleteUser(int id)
        {
            var user = await bookSalesContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {id} không tìm thấy.");
            }

            bookSalesContext.Users.Remove(user);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await bookSalesContext.Users
               .Include(u => u.Role)
               .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                throw new KeyNotFoundException($"User với tên: {username} không tìm thấy.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException("Mật khẩu sai.");
            }

            if(!user.IsActive)
            {
                throw new UnauthorizedAccessException("Tài khoản đã bị khóa");
            }

            return user;
        }

        public async Task ChangePassword(int userId, ChangePasswordRequest changePasswordRequest)
        {
            var user = await bookSalesContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {userId} không tìm thấy.");
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.Password))
            {
                throw new UnauthorizedAccessException("Mật khẩu cũ không chính xác.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);

            bookSalesContext.Users.Update(user);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task SetUserStatus(int userId, bool newStatus)
        {
            var user = await bookSalesContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {userId} không tìm thấy.");
            }

            user.IsActive = newStatus;

            bookSalesContext.Users.Update(user);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task ChangeRole(int userId, int newRoleId)
        {
            var user = await bookSalesContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {userId} không tìm thấy.");
            }

            var role = await bookSalesContext.Roles.FindAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role với ID: {user.RoleId} không tìm thấy.");
            }

            user.RoleId = newRoleId;

            bookSalesContext.Users.Update(user);
            await bookSalesContext.SaveChangesAsync();
        }

    }
}
