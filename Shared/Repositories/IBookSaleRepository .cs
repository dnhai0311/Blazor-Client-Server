using Shared.Models;

namespace Shared.Repositories
{
    public interface IBookSaleRepository
    {
        Task<List<BookSale>> GetAllBookSales();
        Task<BookSale> GetBookSaleById(int id);
        Task AddBookSale(BookSale bookSale);
        Task UpdateBookSale(BookSale bookSale);
        Task DeleteBookSale(int id);
    }
}
