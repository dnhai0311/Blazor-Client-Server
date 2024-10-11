namespace Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username, string roleName);
    }
}
