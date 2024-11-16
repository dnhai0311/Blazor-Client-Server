using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Repositories;
using Shared.Models;
using Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;

namespace Testing
{
    public class BookSaleTesting
    {
        public static List<BookSale> listBookSale { get; set; }
        private IBookSaleRepository bookSaleRepository;
        private BookSalesContext bookSaleContext;
        private Author testAuthor;
        private Mock<IBookSaleRepository> mock;

        public BookSaleTesting()
        {
            testAuthor = new Author { Id = 1, AuthorName = "Dương Ngọc Hải" };
            mock = new Mock<IBookSaleRepository>();

            var options = new DbContextOptionsBuilder<BookSalesContext>()
                .UseInMemoryDatabase(databaseName: "razor_book_manager")
                .Options;

            bookSaleContext = new BookSalesContext(options);
            bookSaleRepository = new BookSaleRepository(bookSaleContext);

            if (!bookSaleContext.BookSales.Any())
            {
                listBookSale = new List<BookSale>
                {
                    new BookSale { Id = 1, Title = "Đường Hầm Mùa Hạ Tập 1", Quantity = 1000, Price = 120000, AuthorId = 1, ImgUrl = "[]" }
                };
                bookSaleContext.Authors.AddRange(testAuthor);
                bookSaleContext.BookSales.AddRange(listBookSale);
                bookSaleContext.SaveChanges();
            }
        }

      
        [Fact]
        public async Task AddBookSale()
        {
           
            var newBook = new BookSale { Title = "New Book", Quantity = 500, Price = 150000, AuthorId = 1, ImgUrl = "[]" };
            mock.Setup(repo => repo.AddBookSale(It.IsAny<BookSale>())).Callback<BookSale>(book =>
            {
                book.Id = 2;
                mock.Setup(repo => repo.GetBookSaleById(book.Id)).ReturnsAsync(book);
            });

            await mock.Object.AddBookSale(newBook);
            await bookSaleRepository.AddBookSale(newBook);
            var result = await bookSaleRepository.GetBookSaleById(2);

            Assert.NotNull(result);
            Assert.Equal(newBook.Title, result.Title);
            Assert.Equal(newBook.Quantity, result.Quantity);
            Assert.Equal(newBook.Price, result.Price);
            Assert.Equal(newBook.AuthorId, result.AuthorId);
            Assert.Equal(newBook.ImgUrl, result.ImgUrl);
        }

        [Fact]
        public async Task GetAllBookSales()
        {
           

            var expectedListBookSale = new List<BookSale>
                {
                    listBookSale[0],
                    new BookSale { Id = 2, Title = "New Book", Quantity = 500, Price = 150000, AuthorId = 1, ImgUrl = "[]" }
                };
            mock.Setup(repo => repo.GetAllBookSales()).ReturnsAsync(listBookSale);

            var result = await bookSaleRepository.GetAllBookSales();

            Assert.NotNull(result);
            Assert.Equal(expectedListBookSale.Count, result.Count);
        }

        [Fact]
        public async Task GetBookSaleById()
        {
           
            var expectedBook = new BookSale { Id = 1, Title = "Updated Title", Quantity = 2000, Price = 150000, AuthorId = 1, ImgUrl = "[UpdatedImgUrl]" };

            mock.Setup(repo => repo.GetBookSaleById(expectedBook.Id)).ReturnsAsync(expectedBook);

            var result = await bookSaleRepository.GetBookSaleById(1);

            Assert.NotNull(result);
            Assert.Equal(expectedBook.Id, result.Id);
            Assert.Equal(expectedBook.Title, result.Title);
            Assert.Equal(expectedBook.Quantity, result.Quantity);
            Assert.Equal(expectedBook.Price, result.Price);
            Assert.Equal(expectedBook.AuthorId, result.AuthorId);
            Assert.Equal(expectedBook.ImgUrl, result.ImgUrl);
        }

        [Fact]
        public async Task UpdateBookSale()
        {
           
            var updatedBook = new BookSale { Id = 1, Title = "Updated Title", Quantity = 2000, Price = 150000, AuthorId = 1, ImgUrl = "[UpdatedImgUrl]" };
            await bookSaleRepository.UpdateBookSale(updatedBook);
            var result = await bookSaleRepository.GetBookSaleById(updatedBook.Id);
            Assert.NotNull(result);
            Assert.Equal(updatedBook.Title, result.Title);
            Assert.Equal(updatedBook.Quantity, result.Quantity);
            Assert.Equal(updatedBook.Price, result.Price);
            Assert.Equal(updatedBook.AuthorId, result.AuthorId);
            Assert.Equal(updatedBook.ImgUrl, result.ImgUrl);
        }

        [Fact]
        public async Task DeleteBookSaleById()
        {
           
            var newBook = new BookSale { Id = 3, Title = "Book to Delete", Quantity = 500, Price = 100000, AuthorId = 1, ImgUrl = "[]" };
            await bookSaleRepository.AddBookSale(newBook);
            var addedBook = await bookSaleRepository.GetBookSaleById(newBook.Id);
            Assert.NotNull(addedBook);
            await bookSaleRepository.DeleteBookSale(newBook.Id);
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await bookSaleRepository.GetBookSaleById(newBook.Id);
            });
        }
    }
}
