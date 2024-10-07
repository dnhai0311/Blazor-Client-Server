using Microsoft.EntityFrameworkCore;
using Server.Models;
using Shared.Models;
using Shared.Repositories;

namespace Server.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookSalesContext bookSalesContext;

        public AuthorRepository(BookSalesContext context)
        {
            bookSalesContext = context;
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await bookSalesContext.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            var author = await bookSalesContext.Authors.FindAsync(id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author với ID: {id} không tìm thấy.");
            }
            return author;
        }

        public async Task<List<BookSale>> GetAllBookSalesFromAuthor(int id)
        {
            var author = await bookSalesContext.Authors
                .Include(a => a.BookSales)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author với ID: {id} không tìm thấy.");
            }
            return author.BookSales.ToList()!;
        }

        public async Task AddAuthor(Author author)
        {
            bookSalesContext.Add(author);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task UpdateAuthor(Author author)
        {
            var existingAuthor = await bookSalesContext.Authors.FindAsync(author.Id);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author với ID: {author.Id} không tìm thấy.");
            }
            bookSalesContext.Entry(existingAuthor).CurrentValues.SetValues(author);
            await bookSalesContext.SaveChangesAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await bookSalesContext.Authors.FindAsync(id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author với ID: {id} không tìm thấy.");
            }
            bookSalesContext.Authors.Remove(author);
            await bookSalesContext.SaveChangesAsync();
        }
    }
}

