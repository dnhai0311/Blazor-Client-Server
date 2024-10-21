using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly BookSalesContext bookSaleContext;
        private readonly IConfiguration configuration;

        public TokenService(BookSalesContext bookSalesContext, IConfiguration configuration)
        {
            bookSaleContext = bookSalesContext;
            this.configuration = configuration;
        }

        public string Key => configuration.GetValue<string>("Jwt:Key")!;
        public string RefreshKey => configuration.GetValue<string>("Jwt:RefreshKey")!;
        public string Issuer => configuration.GetValue<string>("Jwt:Issuer")!;
        public string Audience => configuration.GetValue<string>("Jwt:Audience")!;
        public int TokenExpiry => configuration.GetValue<int>("Jwt:TokenExpiry");
        public int RefreshTokenExpiry => configuration.GetValue<int>("Jwt:RefreshTokenExpiry");

        public string GenerateToken(int id, string username, string roleName, bool isRefreshToken)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = isRefreshToken ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefreshKey))
                                      : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = isRefreshToken ? DateTime.UtcNow.AddDays(RefreshTokenExpiry)
                                         : DateTime.UtcNow.AddMinutes(TokenExpiry);

            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task StoreRefreshToken(int userId, string refreshToken, DateTime expiry)
        {
            var existingToken = await bookSaleContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiresAt = expiry;
                bookSaleContext.RefreshTokens.Update(existingToken);
            }
            else
            {
                var newRefreshToken = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    ExpiresAt = expiry
                };
                await bookSaleContext.RefreshTokens.AddAsync(newRefreshToken);
            }

            await bookSaleContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var token = await bookSaleContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken
                                          && rt.ExpiresAt > DateTime.UtcNow);
            return token != null;
        }


        public async Task RemoveRefreshToken(int userId)
        {
            var token = await bookSaleContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (token == null)
            {
                throw new KeyNotFoundException($"Refresh token không tìm thấy cho userId: {userId}");
            }

            bookSaleContext.RefreshTokens.Remove(token);
            await bookSaleContext.SaveChangesAsync();
        }
    }
}
