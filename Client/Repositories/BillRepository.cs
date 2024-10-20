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
            try
            {
                var bills = await HttpClient.GetFromJsonAsync<List<Bill>>("api/bill");
                return bills ?? new List<Bill>();
            }
            catch
            {
            }
            return new List<Bill>();
        }

        public async Task<Bill> GetAllBillDetailsByBillId(int id)
        {
            try
            {
                var bill = await HttpClient.GetFromJsonAsync<Bill>($"api/bill/{id}");
                return bill ?? new Bill();
            }
            catch
            {
            }
            return new Bill();
        }

        public async Task AddBill(Bill bill)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("api/bill", bill);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
            }
        }
    }
}
