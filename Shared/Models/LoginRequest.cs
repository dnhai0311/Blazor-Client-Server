using System.ComponentModel.DataAnnotations;


namespace Shared.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập Name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name có độ dài từ 5 -> 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Name chỉ cho phép các ký tự a-z, A-Z và số, không cho phép khoảng trắng.")]
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        public string Password { get; set; } = String.Empty;

        public bool RememberMe { get; set; }
    }
}
