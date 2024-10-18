namespace Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(int id , string username, string roleName, bool isRefreshToken);
    }
}
