using Blazor_BookSale_Manager.Models;
using Blazor_BookSale_Manager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blazor_BookSale_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBookSaleRepository bookSaleRepository;

        public BillController(IBookSaleRepository bookSaleRepository)
        {
            this.bookSaleRepository = bookSaleRepository;
        }

        // GET: api/bill
        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var bills = await bookSaleRepository.GetAllBills();
            return Ok(bills);
        }

        // GET: api/bill/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillDetailsByBillId(int id)
        {
            try
            {
                var bill = await bookSaleRepository.GetAllBillDetailsByBillId(id);
                return Ok(bill);
            }
            catch (Exception ex) {
                return NotFound(ex.Message);
            }
        }

        // POST: api/bill
        [HttpPost]
        public async Task<IActionResult> AddBill([FromBody] Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await bookSaleRepository.AddBill(bill);
            return CreatedAtAction(nameof(GetBillDetailsByBillId), new { id = bill.Id }, bill);
        }
    }
}
