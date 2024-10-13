using System.ComponentModel.DataAnnotations;


namespace Shared.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập tên tài khoản")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Tên tài khoản phải có độ dài từ 5 -> 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên tài khoản chỉ cho phép các ký tự a-z, A-Z và số, không cho phép khoảng trắng.")]
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Mật khẩu phải có độ dài từ 5 -> 20 ký tự")]
        public string Password { get; set; } = String.Empty;

        public bool RememberMe { get; set; }
    }
}
