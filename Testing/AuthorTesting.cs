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
    public class AuthorTesting
    {
        public static List<Author> listAuthor { get; set; }
        private IAuthorRepository authorRepository;
        private BookSalesContext bookSalesContext;
        private Mock<IAuthorRepository> mock;

        public AuthorTesting()
        {
            mock = new Mock<IAuthorRepository>();

            var options = new DbContextOptionsBuilder<BookSalesContext>()
                .UseInMemoryDatabase(databaseName: "razor_book_manager.")
                .Options;

            bookSalesContext = new BookSalesContext(options);
            authorRepository = new AuthorRepository(bookSalesContext);

            if (!bookSalesContext.Authors.Any())
            {
                listAuthor = new List<Author>
                {
                    new Author { Id = 1, AuthorName = "Dương Ngọc Hải" }
                };
                bookSalesContext.Authors.AddRange(listAuthor);
                bookSalesContext.SaveChanges();
            }
        }

        [Fact]
        public async Task AddAuthor()
        {
            var newAuthor = new Author { AuthorName = "Nguyễn Nhật Ánh" };
            mock.Setup(repo => repo.AddAuthor(It.IsAny<Author>())).Callback<Author>(author =>
            {
                author.Id = 2;
                mock.Setup(repo => repo.GetAuthorById(author.Id)).ReturnsAsync(author);
            });

            await mock.Object.AddAuthor(newAuthor);
            await authorRepository.AddAuthor(newAuthor);
            var result = await authorRepository.GetAuthorById(2);

            Assert.NotNull(result);
            Assert.Equal(newAuthor.AuthorName, result.AuthorName);
        }

        [Fact]
        public async Task GetAllAuthors()
        {
            var expectedListAuthor = new List<Author>
            {
                listAuthor[0],
                new Author { Id = 2, AuthorName = "Nguyễn Nhật Ánh" }
            };
            mock.Setup(repo => repo.GetAllAuthors()).ReturnsAsync(listAuthor);

            var result = await authorRepository.GetAllAuthors();

            Assert.NotNull(result);
            Assert.Equal(expectedListAuthor.Count, result.Count);
        }

        [Fact]
        public async Task GetAuthorById()
        {
            var expectedAuthor = new Author { Id = 1, AuthorName = "Dương Ngọc Hải" };

            mock.Setup(repo => repo.GetAuthorById(expectedAuthor.Id)).ReturnsAsync(expectedAuthor);

            var result = await authorRepository.GetAuthorById(1);

            Assert.NotNull(result);
            Assert.Equal(expectedAuthor.Id, result.Id);
            Assert.Equal(expectedAuthor.AuthorName, result.AuthorName);
        }

        [Fact]
        public async Task UpdateAuthor()
        {
            var updatedAuthor = new Author { Id = 1, AuthorName = "Updated Author Name" };
            await authorRepository.UpdateAuthor(updatedAuthor);
            var result = await authorRepository.GetAuthorById(updatedAuthor.Id);
            Assert.NotNull(result);
            Assert.Equal(updatedAuthor.AuthorName, result.AuthorName);
        }

        [Fact]
        public async Task DeleteAuthorById()
        {
            var newAuthor = new Author { Id = 3, AuthorName = "Author to Delete" };
            await authorRepository.AddAuthor(newAuthor);
            var addedAuthor = await authorRepository.GetAuthorById(newAuthor.Id);
            Assert.NotNull(addedAuthor);
            await authorRepository.DeleteAuthor(newAuthor.Id);
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await authorRepository.GetAuthorById(newAuthor.Id);
            });
        }
    }
}
