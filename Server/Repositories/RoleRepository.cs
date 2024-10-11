using Microsoft.EntityFrameworkCore;
using Server.Models;
using Shared.Models;
using Shared.Repositories;

namespace Server.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BookSalesContext BookSaleContext;

        public RoleRepository(BookSalesContext context)
        {
            BookSaleContext = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await BookSaleContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            var role = await BookSaleContext.Roles.FindAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role với ID: {id} không tìm thấy.");
            }
            return role;
        }

        public async Task AddRole(Role role)
        {
            var existingRole = await BookSaleContext.Roles
                .FirstOrDefaultAsync(r => r.RoleName == role.RoleName);

            if (existingRole != null)
            {
                throw new InvalidOperationException("Role với tên này đã tồn tại.");
            }

            BookSaleContext.Add(role);
            await BookSaleContext.SaveChangesAsync();
        }

        public async Task UpdateRole(Role role)
        {
            var existingRole = await BookSaleContext.Roles.FindAsync(role.Id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"Role với ID: {role.Id} không tìm thấy.");
            }

            var otherRole = await BookSaleContext.Roles
                .FirstOrDefaultAsync(r => r.RoleName == role.RoleName && r.Id != role.Id);

            if (otherRole != null)
            {
                throw new InvalidOperationException("Role với tên này đã tồn tại.");
            }

            BookSaleContext.Entry(existingRole).CurrentValues.SetValues(role);
            await BookSaleContext.SaveChangesAsync();
        }

        public async Task DeleteRole(int id)
        {
            var role = await BookSaleContext.Roles.FindAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role với ID: {id} không tìm thấy.");
            }
            BookSaleContext.Roles.Remove(role);
            await BookSaleContext.SaveChangesAsync();
        }
    }
}
