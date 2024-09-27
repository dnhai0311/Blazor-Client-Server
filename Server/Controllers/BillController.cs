using Server.Models;
using Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBookSaleRepository BookSaleRepository;

        public BillController(IBookSaleRepository bookSaleRepository)
        {
            this.BookSaleRepository = bookSaleRepository;
        }

        // GET: api/bill
        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var bills = await BookSaleRepository.GetAllBills();
            return Ok(bills);
        }

        // GET: api/bill/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillDetailsByBillId(int id)
        {
            try
            {
                var bill = await BookSaleRepository.GetAllBillDetailsByBillId(id);
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

            await BookSaleRepository.AddBill(bill);
            return CreatedAtAction(nameof(GetBillDetailsByBillId), new { id = bill.Id }, bill);
        }
    }
}
