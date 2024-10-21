namespace Server.Services
{
    public interface ITokenService
    {
        string RefreshKey { get; }
        string Issuer { get; }
        string Audience { get; }
        int RefreshTokenExpiry { get; }
        string GenerateToken(int id, string username, string roleName, bool isRefreshToken);
        Task StoreRefreshToken(int userId, string refreshToken, DateTime expiry);
        Task<bool> ValidateRefreshToken(string refreshToken);
        Task RemoveRefreshToken(int userId);
    }
}