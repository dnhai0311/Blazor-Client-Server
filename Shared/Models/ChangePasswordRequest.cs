using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải nhập Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;


        [Required(ErrorMessage = "Bạn phải nhập lại Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password có độ dài từ 5 -> 20 ký tự")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng nhau")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
