using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class UserRepository : IUserClientRepository
    {
        private readonly HttpClient HttpClient;

        public UserRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await HttpClient.GetFromJsonAsync<List<User>>("api/user") ?? new List<User>();
        }

        public async Task<User> GetUserById(int id)
        {
            var response = await HttpClient.GetAsync($"api/user/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>() ?? new User();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task AddUser(RegisterRequest registerRequest)
        {
            var response = await HttpClient.PostAsJsonAsync("api/user", registerRequest);

            var registerResult = await response.Content.ReadFromJsonAsync<RegisterResult>();

            if (registerResult != null && !registerResult.Successful)
            {
                throw new ApplicationException(registerResult?.Errors[0]);
            }
            if(!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task UpdateUser(User user)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/user/{user.Id}", user);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task DeleteUser(int id)
        {
            var response = await HttpClient.DeleteAsync($"api/user/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }
    }
}
