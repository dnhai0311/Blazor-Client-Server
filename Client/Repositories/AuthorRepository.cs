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
            return await httpClient.GetFromJsonAsync<List<Author>>("api/author");
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await httpClient.GetFromJsonAsync<Author>($"api/author/{id}");
            if (author == null)
            {
                throw new Exception($"Author với ID: {id} không thấy.");
            }
            return author;
        }

        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId)
        {
            return await httpClient.GetFromJsonAsync<List<BookSale>>($"api/author/{authorId}/booksales");
        }

        public async Task AddAuthor(Author author)
        {
            var response = await httpClient.PostAsJsonAsync("api/author", author);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAuthor(Author author)
        {
            var response = await httpClient.PutAsJsonAsync($"api/author/{author.Id}", author);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAuthor(int id)
        {
            var response = await httpClient.DeleteAsync($"api/author/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
