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
            return await bookSalesContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await bookSalesContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User với ID: {id} không tìm thấy.");
            }
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

            return user;
        }
    }
}
