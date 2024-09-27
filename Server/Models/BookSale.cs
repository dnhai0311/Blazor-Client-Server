using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class BookSale
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Bạn phải nhập Title")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Title có độ dài từ 5 -> 50 ký tự")]
        public string Title { get; set; } = String.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0")]
        public int Quantity { get; set; } = 0;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price phải lớn hơn 0")]
        public double Price { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Bạn phải chọn Author")]
        public int AuthorId { get; set; } = 0;
        public Author? Author { get; set; }
        [JsonIgnore]
        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
    }
}
