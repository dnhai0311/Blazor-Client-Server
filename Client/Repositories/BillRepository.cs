using Shared.Models;
using Shared.Repositories;

namespace Client.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly HttpClient HttpClient;

        public BillRepository(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public async Task<List<Bill>> GetAllBills()
        {
            var bills = await HttpClient.GetFromJsonAsync<List<Bill>>("api/bill");
            return bills ?? new List<Bill>();
        }


        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            var bill = await HttpClient.GetFromJsonAsync<Bill>($"api/bill/{id}");
            return bill ?? new Bill();
        }

        public async Task AddBill(Bill bill)
        {
            var response = await HttpClient.PostAsJsonAsync("api/bill", bill);
            response.EnsureSuccessStatusCode();
        }
    }
}
