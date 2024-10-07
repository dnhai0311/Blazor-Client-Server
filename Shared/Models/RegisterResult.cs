namespace Shared.Models
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
