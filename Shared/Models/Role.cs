using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập RoleName")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "RoleName có độ dài từ 4 -> 20 ký tự")]
        public string RoleName { get; set; } = string.Empty; 

        public List<User> Users { get; set; } = new List<User>();
    }
}
