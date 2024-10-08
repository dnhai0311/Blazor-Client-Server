namespace Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string username, int role);
    }
}
