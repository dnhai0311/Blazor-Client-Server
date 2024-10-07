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
            var bills = await httpClient.GetFromJsonAsync<List<Bill>>("api/bill");
            return bills ?? new List<Bill>();
        }


        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await httpClient.GetFromJsonAsync<Bill>($"api/bill/{id}");
            return bill ?? new Bill();
        }

        public async Task AddBill(Bill bill)
        {
            var response = await httpClient.PostAsJsonAsync("api/bill", bill);
            response.EnsureSuccessStatusCode();
        }
    }
}
