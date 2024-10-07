using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Bạn phải nhập Name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name có độ dài từ 5 -> 20 ký tự")] 
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        public string Password { get; set; } = String.Empty;
    }
}
