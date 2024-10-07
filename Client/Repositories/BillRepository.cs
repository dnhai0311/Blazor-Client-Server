using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly HttpClient httpClient;

        public BillRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Bill>> GetAllBills()
        {
            return await httpClient.GetFromJsonAsync<List<Bill>>("api/bill");
        }

        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await httpClient.GetFromJsonAsync<Bill>($"api/bill/{id}");
            if (bill == null)
            {
                throw new Exception($"Bill với ID: {id} không thấy.");
            }
            return bill;
        }

        public async Task AddBill(Bill bill)
        {
            var response = await httpClient.PostAsJsonAsync("api/bill", bill);
            response.EnsureSuccessStatusCode();
        }
    }
}
