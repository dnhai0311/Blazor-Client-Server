using Microsoft.EntityFrameworkCore;
using Server.Models;
using Shared.Models;
using Shared.Repositories;

namespace Server.Repositories
{
    public class BookSaleRepository : IBookSaleRepository
    {
        private readonly BookSalesContext bookSalesContext;

        public BookSaleRepository(BookSalesContext context)
        {
            bookSalesContext = context;
        }

        public async Task<List<BookSale>> GetAllBookSales()
        {
            return await bookSalesContext.BookSales
                .OrderBy(sale => sale.Id)
                .Include(sale => sale.Author)
                .ToListAsync();
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var bookSale = await bookSalesContext.BookSales.FindAsync(id);
            if (bookSale == null)
            {
                throw new KeyNotFoundException($"BookSale với ID: {id} không tìm thấy.");
            }
            return bookSale;
        }

        public async Task AddBookSale(BookSale bookSale)
        {
            var existingBookSale = await bookSalesContext.BookSales
                .FirstOrDefaultAsync(b => b.Title == bookSale.Title);
            if (existingBookSale != null)
            {
                throw new InvalidOperationException("BookSale với tiêu đề này đã tồn tại.");
            }

            var existingAuthor = await bookSalesContext.Authors.FindAsync(bookSale.AuthorId);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author với ID: {bookSale.AuthorId} không tìm thấy.");
            }

            bookSalesContext.Add(bookSale);
            await bookSalesContext.SaveChangesAsync();
        }


        public async Task UpdateBookSale(BookSale bookSale)
        {
            var existingBookSale = await bookSalesContext.BookSales.FindAsync(bookSale.Id);
            if (existingBookSale == null)
            {
                throw new KeyNotFoundException($"BookSale với ID: {bookSale.Id} không tìm thấy.");
            }

            var otherBookSale = await bookSalesContext.BookSales
                .FirstOrDefaultAsync(b => b.Title == bookSale.Title && b.Id != bookSale.Id);
            if (otherBookSale != null)
            {
                throw new InvalidOperationException("BookSale với tiêu đề này đã tồn tại.");
            }

            var existingAuthor = await bookSalesContext.Authors.FindAsync(bookSale.AuthorId);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author với ID: {bookSale.AuthorId} không tìm thấy.");
            }

            bookSalesContext.Entry(existingBookSale).CurrentValues.SetValues(bookSale);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task DeleteBookSale(int id)
        {
            var bookSale = await bookSalesContext.BookSales.FindAsync(id);
            if (bookSale == null)
            {
                throw new KeyNotFoundException($"BookSale với ID: {id} không tìm thấy.");
            }

            bookSalesContext.BookSales.Remove(bookSale);
            await bookSalesContext.SaveChangesAsync();
        }
    }
}
