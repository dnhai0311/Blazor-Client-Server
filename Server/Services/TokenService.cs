using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
    public class TokenService : ITokenService
    {
        public string Key { get; private set; }
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public int ExpiryInDays { get; private set; }

        public TokenService(IConfiguration configuration)
        {
            Key = configuration.GetValue<string>("Jwt:Key")!;
            Issuer = configuration.GetValue<string>("Jwt:Issuer")!;
            Audience = configuration.GetValue<string>("Jwt:Audience")!;
            ExpiryInDays = configuration.GetValue<int>("Jwt:ExpiryInDays");
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
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
