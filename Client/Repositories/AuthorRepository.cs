using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly HttpClient HttpClient;

        public AuthorRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            var authors = await HttpClient.GetFromJsonAsync<List<Author>>("api/author");
            return authors ?? new List<Author>();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var response = await HttpClient.GetAsync($"api/author/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Author>() ?? new Author();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }


        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId)
        {
            var response = await HttpClient.GetAsync($"api/author/{authorId}/booksales");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<BookSale>>() ?? new List<BookSale>();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }



        public async Task AddAuthor(Author author)
        {
            var response = await HttpClient.PostAsJsonAsync("api/author", author);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }


        public async Task UpdateAuthor(Author author)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/author/{author.Id}", author);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task DeleteAuthor(int id)
        {
            var response = await HttpClient.DeleteAsync($"api/author/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
