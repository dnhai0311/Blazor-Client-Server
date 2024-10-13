using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập tên tài khoản")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Tên tài khoản phải có độ dài từ 5 -> 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Têb tài khoản chỉ cho phép các ký tự a-z, A-Z và số, không cho phép khoảng trắng.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Mật khẩu phải có độ dài từ 5 -> 20 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập lại mật khẩu")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Mật khẩu phải có độ dài từ 5 -> 20 ký tự")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng nhau")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Bạn phải chọn vai trò")]
        public int RoleId { get; set; }
    }
}
