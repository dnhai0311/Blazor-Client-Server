using Blazor_BookSale_Manager.Models;

namespace Blazor_BookSale_Manager.Repositories
{
    public interface IBookSaleRepository
    {
        Task<List<BookSale>> GetAllBookSales();
        Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId);
        Task<List<Author>> GetAllAuthors();
        Task<BookSale> GetBookSaleById(int id);
        Task<List<Bill>> GetAllBills();
        Task<Bill> GetAllBillDetailsByBillId(int id);
        Task AddBookSale(BookSale bookSale);
        Task UpdateBookSale(BookSale bookSale);
        Task DeleteBookSale(int id);
        Task<Author> GetAuthorById(int id);
        Task AddAuthor(Author author);
        Task UpdateAuthor(Author author);
        Task DeleteAuthor(int id);
        Task AddBill(Bill bill);
    }
}
