﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên tác giả.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên tác giả phải có độ dài từ 5 -> 50 ký tự")]
        public string AuthorName { get; set; } = string.Empty;

        [JsonIgnore]
        public List<BookSale> BookSales { get; set; } = new List<BookSale>();
    }
}
