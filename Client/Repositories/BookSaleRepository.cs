using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class BookSaleRepository : IBookSaleRepository
    {
        private readonly HttpClient HttpClient;

        public BookSaleRepository(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<List<BookSale>> GetAllBookSales()
        {
            return await HttpClient.GetFromJsonAsync<List<BookSale>>("api/booksale") ?? new List<BookSale>();
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var response = await HttpClient.GetAsync($"api/booksale/{id}");

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
            var response = await HttpClient.PostAsJsonAsync("api/booksale", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task UpdateBookSale(BookSale bookSale)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/booksale/{bookSale.Id}", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Lỗi từ API: {errorMessage}");
            }
        }

        public async Task DeleteBookSale(int id)
        {
            var response = await HttpClient.DeleteAsync($"api/booksale/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
