namespace Shared.Models
{
    public class User
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = String.Empty;
        public bool IsAuthenticated { get; set; }
    }
}
