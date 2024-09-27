using Blazor_BookSale_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_BookSale_Manager.Repositories
{
    public class BookSaleRepository : IBookSaleRepository
    {
        BookSalesContext bookSalesContext;
        public BookSaleRepository(BookSalesContext context)
        {
            this.bookSalesContext = context;
        }
        public async Task<List<BookSale>> GetAllBookSales()
        {
            return await this.bookSalesContext.BookSales
                .Include(sale => sale.Author)
                .ToListAsync();
        }
        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int authorId)
        {
            var author = await this.bookSalesContext.Authors
                .Include(a => a.BookSales)
                .FirstOrDefaultAsync(a => a.Id == authorId);

            return author?.BookSales.ToList() ?? new List<BookSale>();
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await this.bookSalesContext.Authors.ToListAsync();
        }

        public async Task<BookSale> GetBookSaleById(int id)
        {
            var bookSale = await this.bookSalesContext.BookSales.FindAsync(id);

            if (bookSale == null)
            {
                throw new ($"BookSale với ID: {id} không thấy.");
            }

            return bookSale;
        }

        public async Task<List<Bill>> GetAllBills()
        {
            return await this.bookSalesContext.Bills.ToListAsync();
        }

        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await this.bookSalesContext.Bills
                .Include(b => b.BillDetails)
                    .ThenInclude(bd => bd.BookSale)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                Console.WriteLine("Null");
            }

            return bill ?? new Bill(); 
        }


        public async Task AddBookSale(BookSale bookSale)
        {
            this.bookSalesContext.Add(bookSale);
            await this.bookSalesContext.SaveChangesAsync();
        }
        public async Task UpdateBookSale(BookSale bookSale)
        {
            var existingBookSale = await bookSalesContext.BookSales.FindAsync(bookSale.Id);
            if (existingBookSale != null)
            {
                existingBookSale.Title = bookSale.Title;
                existingBookSale.Quantity = bookSale.Quantity;
                existingBookSale.Price = bookSale.Price;
                await bookSalesContext.SaveChangesAsync();
            }
        }
        public async Task DeleteBookSale(int id)
        {
            var bookSale = await bookSalesContext.BookSales.FindAsync(id);
            if (bookSale != null)
            {
                bookSalesContext.BookSales.Remove(bookSale);
                await bookSalesContext.SaveChangesAsync();
            }
        }


        public async Task<Author> GetAuthorById(int id)
        {
            var author = await this.bookSalesContext.Authors.FindAsync(id);

            if (author == null)
            {
                throw new($"BookSale với ID: {id} không thấy.");
            }

            return author;
        }

        public async Task AddAuthor(Author author)
        {
            this.bookSalesContext.Add(author);
            await this.bookSalesContext.SaveChangesAsync();
        }
        public async Task UpdateAuthor(Author author)
        {
            var existingAuthor = await bookSalesContext.Authors.FindAsync(author.Id);
            if (existingAuthor != null)
            {
                existingAuthor.AuthorName = author.AuthorName;
                await bookSalesContext.SaveChangesAsync();
            }
        }
        public async Task DeleteAuthor(int id)
        {
            var author = await bookSalesContext.Authors.FindAsync(id);
            if (author != null)
            {
                bookSalesContext.Authors.Remove(author);
                await bookSalesContext.SaveChangesAsync();
            }
        }

        public async Task AddBill(Bill bill)
        {
            foreach (var detail in bill.BillDetails)
            {
                var bookSale = await this.bookSalesContext.BookSales
                    .FindAsync(detail.BookSaleId);

                if (bookSale == null)
                {
                    return;
                }

                bookSale.Quantity -= detail.Quantity;
            }

            this.bookSalesContext.Bills.Add(bill);
            await this.bookSalesContext.SaveChangesAsync();
        }



    }
}
