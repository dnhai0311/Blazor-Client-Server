using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Bạn phải nhập Name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name có độ dài từ 5 -> 20 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Name chỉ cho phép các ký tự a-z, A-Z và số, không cho phép khoảng trắng.")]
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        public string Password { get; set; } = String.Empty;

        [Required(ErrorMessage = "Bạn phải chọn Role")]
        public int RoleId { get; set; } = 0;

        public bool isActive { get; set; } = true;

        public Role? Role { get; set; }
    }
}
