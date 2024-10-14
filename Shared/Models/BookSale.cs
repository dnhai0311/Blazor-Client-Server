using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class BookSale
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Bạn phải nhập tên sách")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên sách có độ dài từ 5 -> 50 ký tự")]
        public string Title { get; set; } = String.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; } = 0;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public double Price { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Bạn phải chọn tác giả")]
        public int AuthorId { get; set; } = 0;
        [Required]
        public string ImgUrl { get; set; } = String.Empty;
        [Required]
        public string Description { get; set; } = String.Empty;
        public Author? Author { get; set; }
        [JsonIgnore]
        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
    }
}
