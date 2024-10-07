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
            return await httpClient.GetFromJsonAsync<List<BookSale>>("api/booksale") ?? new List<BookSale>();
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var response = await httpClient.GetAsync($"api/booksale/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BookSale>() ?? new BookSale();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task AddBookSale(BookSale bookSale)
        {
            var response = await httpClient.PostAsJsonAsync("api/booksale", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task UpdateBookSale(BookSale bookSale)
        {
            var response = await httpClient.PutAsJsonAsync($"api/booksale/{bookSale.Id}", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task DeleteBookSale(int id)
        {
            var response = await httpClient.DeleteAsync($"api/booksale/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
