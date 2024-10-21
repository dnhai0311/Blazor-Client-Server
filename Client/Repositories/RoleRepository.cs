using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly HttpClient httpClient;

        public RoleRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var roles = await httpClient.GetFromJsonAsync<List<Role>>("api/role");
                return roles ?? new List<Role>();
            }
            catch
            {
            }
            return new List<Role>();
        }

        public async Task<Role> GetRoleById(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/role/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Role>() ?? new Role();
                }
            }
            catch
            {
            }
            return new Role();
        }

        public async Task AddRole(Role role)
        {
            var response = await httpClient.PostAsJsonAsync("api/role", role);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task UpdateRole(Role role)
        {
            var response = await httpClient.PutAsJsonAsync($"api/role/{role.Id}", role);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }

        }

        public async Task DeleteRole(int id)
        {
            var response = await httpClient.DeleteAsync($"api/role/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
