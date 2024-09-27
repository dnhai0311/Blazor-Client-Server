namespace Client.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public Bill? Bill { get; set; }

        public int BookSaleId { get; set; }
        public BookSale? BookSale { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
