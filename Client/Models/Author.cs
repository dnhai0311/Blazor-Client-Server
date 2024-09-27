using System.ComponentModel.DataAnnotations;

namespace Blazor_BookSale_Manager.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên tác giả.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Author Name có độ dài từ 5 -> 50 ký tự")]
        public string AuthorName { get; set; } = string.Empty;

        public List<BookSale> BookSales { get; set; } = new List<BookSale>();
    }
}
