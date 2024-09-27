using Blazor_BookSale_Manager.Models;
using Blazor_BookSale_Manager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blazor_BookSale_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSaleController : ControllerBase
    {
        private readonly IBookSaleRepository BooksalesRepository;

        public BookSaleController(IBookSaleRepository bookSaleRepository)
        {
            BooksalesRepository = bookSaleRepository;
        }

        // GET: api/booksale
        [HttpGet]
        public async Task<IActionResult> GetAllbooksales()
        {
            var bookSales = await BooksalesRepository.GetAllBookSales();
            return Ok(bookSales);
        }

        // GET: api/booksale/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookSaleById(int id)
        {
            try
            {
                var bookSale = await BooksalesRepository.GetBookSaleById(id);
                return Ok(bookSale);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/booksale
        [HttpPost]
        public async Task<IActionResult> AddBookSale([FromBody] BookSale bookSale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await BooksalesRepository.AddBookSale(bookSale);
            return CreatedAtAction(nameof(GetBookSaleById), new { id = bookSale.Id }, bookSale);
        }

        // PUT: api/booksale/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookSale(int id, [FromBody] BookSale bookSale)
        {
            if (!ModelState.IsValid || bookSale.Id != id)
            {
                return BadRequest(ModelState);
            }

            await BooksalesRepository.UpdateBookSale(bookSale);
            return NoContent();
        }

        // DELETE: api/booksale/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookSale(int id)
        {
            await BooksalesRepository.DeleteBookSale(id);
            return NoContent();
        }
    }
}
