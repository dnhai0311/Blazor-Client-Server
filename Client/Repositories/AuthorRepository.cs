using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly HttpClient httpClient;

        public AuthorRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            var authors = await httpClient.GetFromJsonAsync<List<Author>>("api/author");
            return authors ?? new List<Author>();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var response = await httpClient.GetAsync($"api/author/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Author>() ?? new Author();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }


        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId)
        {
            var response = await httpClient.GetAsync($"api/author/{authorId}/booksales");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<BookSale>>() ?? new List<BookSale>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }



        public async Task AddAuthor(Author author)
        {
            var response = await httpClient.PostAsJsonAsync("api/author", author);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }


        public async Task UpdateAuthor(Author author)
        {
            var response = await httpClient.PutAsJsonAsync($"api/author/{author.Id}", author);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task DeleteAuthor(int id)
        {
            var response = await httpClient.DeleteAsync($"api/author/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
