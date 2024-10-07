using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class BookSaleRepository : IBookSaleRepository
    {
        private readonly HttpClient httpClient;

        public BookSaleRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<BookSale>> GetAllBookSales()
        {
            return await httpClient.GetFromJsonAsync<List<BookSale>>("api/booksale");
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var bookSale = await httpClient.GetFromJsonAsync<BookSale>($"api/booksale/{id}");
            if (bookSale == null)
            {
                throw new Exception($"BookSale với ID: {id} không thấy.");
            }
            return bookSale;
        }

        public async Task AddBookSale(BookSale bookSale)
        {
            var response = await httpClient.PostAsJsonAsync("api/booksale", bookSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateBookSale(BookSale bookSale)
        {
            var response = await httpClient.PutAsJsonAsync($"api/booksale/{bookSale.Id}", bookSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookSale(int id)
        {
            var response = await httpClient.DeleteAsync($"api/booksale/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
