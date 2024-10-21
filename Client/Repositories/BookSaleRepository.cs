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
            try
            {
                return await HttpClient.GetFromJsonAsync<List<BookSale>>("api/booksale") ?? new List<BookSale>();
            }
            catch
            {
            }
            return new List<BookSale>();
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            try
            {
                var response = await HttpClient.GetAsync($"api/booksale/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookSale>() ?? new BookSale();
                }
            }
            catch
            {
            }
            return new BookSale();
        }

        public async Task AddBookSale(BookSale bookSale)
        {

            var response = await HttpClient.PostAsJsonAsync("api/booksale", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task UpdateBookSale(BookSale bookSale)
        {
            var response = await HttpClient.PutAsJsonAsync($"api/booksale/{bookSale.Id}", bookSale);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task DeleteBookSale(int id)
        {
            var response = await HttpClient.DeleteAsync($"api/booksale/{id}");
            response.EnsureSuccessStatusCode();

        }
    }
}
