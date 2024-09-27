using Client.Models;

namespace Client.Repositories
{
    public class BookSaleRepository : IBookSaleRepository
    {
        private readonly HttpClient _httpClient;

        public BookSaleRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<BookSale>> GetAllBookSales()
        {
            return await _httpClient.GetFromJsonAsync<List<BookSale>>("api/booksale");
        }

        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId)
        {
            return await _httpClient.GetFromJsonAsync<List<BookSale>>($"api/author/{authorId}/booksales");
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await _httpClient.GetFromJsonAsync<List<Author>>("api/author");
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var bookSale = await _httpClient.GetFromJsonAsync<BookSale>($"api/booksale/{id}");
            if (bookSale == null)
            {
                throw new Exception($"BookSale với ID: {id} không thấy.");
            }
            return bookSale;
        }

        public async Task<List<Bill>> GetAllBills()
        {
            return await _httpClient.GetFromJsonAsync<List<Bill>>("api/bill");
        }

        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await _httpClient.GetFromJsonAsync<Bill>($"api/bill/{id}");
            if (bill == null)
            {
                throw new Exception($"Bill với ID: {id} không thấy.");
            }
            return bill;
        }

        public async Task AddBookSale(BookSale bookSale)
        {
            var response = await _httpClient.PostAsJsonAsync("api/booksale", bookSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateBookSale(BookSale bookSale)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/booksale/{bookSale.Id}", bookSale);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBookSale(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/booksale/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await _httpClient.GetFromJsonAsync<Author>($"api/author/{id}");
            if (author == null)
            {
                throw new Exception($"Author với ID: {id} không thấy.");
            }
            return author;
        }

        public async Task AddAuthor(Author author)
        {
            var response = await _httpClient.PostAsJsonAsync("api/author", author);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAuthor(Author author)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/author/{author.Id}", author);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAuthor(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/author/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task AddBill(Bill bill)
        {
            var response = await _httpClient.PostAsJsonAsync("api/bill", bill);
            response.EnsureSuccessStatusCode();
        }
    }
}
