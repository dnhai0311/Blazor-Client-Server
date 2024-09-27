using System.Text.Json.Serialization;

namespace Blazor_BookSale_Manager.Models
{
    public class Bill
    {
        public int Id { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public double TotalPrice { get; set; } = 0;

        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
    }
}
