using Shared.Models;
using Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillRepository BillRepository;

        public BillController(IBillRepository billRepository)
        {
            this.BillRepository = billRepository;
        }

        // GET: api/bill
        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {
            var bills = await BillRepository.GetAllBills();
            return Ok(bills);
        }

        // GET: api/bill/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillDetailsByBillId(int id)
        {
            try
            {
                var bill = await BillRepository.GetAllBillDetailsByBillId(id);
                return Ok(bill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
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

            try
            {
                foreach (var item in bill.BillDetails)
                {
                    item.BookSale = null;
                }

                await BillRepository.AddBill(bill);
                return CreatedAtAction(nameof(GetBillDetailsByBillId), new { id = bill.Id }, bill);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
