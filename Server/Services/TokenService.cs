using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string Key { get; private set; } = configuration.GetValue<string>("Jwt:Key")!;
        public string RefreshKey { get; private set; } = configuration.GetValue<string>("Jwt:RefreshKey")!;
        public string Issuer { get; private set; } = configuration.GetValue<string>("Jwt:Issuer")!;
        public string Audience { get; private set; } = configuration.GetValue<string>("Jwt:Audience")!;
        public int TokenExpiry { get; private set; } = configuration.GetValue<int>("Jwt:TokenExpiry");
        public int RefreshTokenExpiry { get; private set; } = configuration.GetValue<int>("Jwt:RefreshTokenExpiry");


        public string GenerateToken(int id ,string username, string roleName, bool isRefreshToken)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = !isRefreshToken ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)) 
                            : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefreshKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = !isRefreshToken ? DateTime.Now.AddMinutes(Convert.ToInt32(TokenExpiry)) 
                            : DateTime.Now.AddDays(Convert.ToInt32(RefreshTokenExpiry));

            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: expiry,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
