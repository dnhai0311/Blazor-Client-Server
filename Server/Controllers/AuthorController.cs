using Server.Models;
using Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IBookSaleRepository BookSaleRepository;

        public AuthorController(IBookSaleRepository bookSaleRepository)
        {
            BookSaleRepository = bookSaleRepository;
        }

        // GET: api/author
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await BookSaleRepository.GetAllAuthors();
            return Ok(authors);
        }

        // GET: api/author/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var author = await BookSaleRepository.GetAuthorById(id);
                return Ok(author);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/author
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await BookSaleRepository.AddAuthor(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        // PUT: api/author/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
        {
            if (!ModelState.IsValid || author.Id != id)
            {
                return BadRequest(ModelState);
            }

            await BookSaleRepository.UpdateAuthor(author);
            return NoContent();
        }

        // DELETE: api/author/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await BookSaleRepository.DeleteAuthor(id);
            return NoContent();
        }

        // GET: api/author/{id}/booksales
        [HttpGet("{id}/booksales")]
        public async Task<IActionResult> GetAllBookSalesFromAuthor(int id)
        {
            try
            {
                var bookSales = await BookSaleRepository.GetAllBookSalesFromAuthor(id);
                return Ok(bookSales);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

    }
}
