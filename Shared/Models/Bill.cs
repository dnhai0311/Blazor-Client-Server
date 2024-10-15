namespace Shared.Models
{
    public class Bill
    {
        public int Id { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public int UserId { get; set; } = 0;

        public double TotalPrice { get; set; } = 0;

        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

        public User? User { get; set; } = new User();
    }
}
