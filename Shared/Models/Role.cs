using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên vai trò")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Tên vai trò có độ dài từ 4 -> 20 ký tự")]
        public string RoleName { get; set; } = string.Empty;

        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
