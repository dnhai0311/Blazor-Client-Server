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
        public string Issuer { get; private set; } = configuration.GetValue<string>("Jwt:Issuer")!;
        public string Audience { get; private set; } = configuration.GetValue<string>("Jwt:Audience")!;
        public int ExpiryInDays { get; private set; } = configuration.GetValue<int>("Jwt:ExpiryInDays");

        public string GenerateToken(int id ,string username, string roleName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, roleName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(ExpiryInDays));
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
