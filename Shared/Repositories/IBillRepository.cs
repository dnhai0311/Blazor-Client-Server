using Shared.Models;

namespace Shared.Repositories
{
    public interface IBillRepository
    {
        Task<List<Bill>> GetAllBills();
        Task<Bill> GetAllBillDetailsByBillId(int id);
        Task AddBill(Bill bill);
    }
}
