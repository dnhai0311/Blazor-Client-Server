using Shared.Models;
using Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BookSaleController : ControllerBase
    {
        private readonly IBookSaleRepository BookSalesRepository;

        public BookSaleController(IBookSaleRepository bookSaleRepository)
        {
            BookSalesRepository = bookSaleRepository;
        }

        // GET: api/booksale
        [HttpGet]
        public async Task<IActionResult> GetAllBooksales()
        {
            var bookSales = await BookSalesRepository.GetAllBookSales();
            return Ok(bookSales);
        }

        // GET: api/booksale/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookSaleById(int id)
        {
            try
            {
                var bookSale = await BookSalesRepository.GetBookSaleById(id);
                return Ok(bookSale);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}"); 
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

            try
            {
                bookSale.Author = null;
                await BookSalesRepository.AddBookSale(bookSale);
                return CreatedAtAction(nameof(GetBookSaleById), new { id = bookSale.Id }, bookSale);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        // PUT: api/booksale/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookSale(int id, [FromBody] BookSale bookSale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookSale.Id)
            {
                return BadRequest("ID trong URL không khớp với ID của sách.");
            }

            try
            {
                bookSale.Author = null;
                await BookSalesRepository.UpdateBookSale(bookSale);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        // DELETE: api/booksale/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookSale(int id)
        {
            try
            {
                await BookSalesRepository.DeleteBookSale(id);
                return NoContent(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}"); 
            }
        }
    }
}
