using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Repositories;
using Shared.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserServerRepository UserRepository;
    private readonly ITokenService TokenService;
    private readonly IConfiguration Configuration;

    public AuthController(IUserServerRepository userRepository, ITokenService tokenService, IConfiguration configuration)
    {
        UserRepository = userRepository;
        TokenService = tokenService;
        Configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var authenticatedUser = await UserRepository.Authenticate(loginRequest.UserName, loginRequest.Password);

            var token = TokenService.GenerateToken(authenticatedUser.Id, authenticatedUser.UserName,
                                    authenticatedUser.Role?.RoleName!, isRefreshToken: false);

            var refreshToken = TokenService.GenerateToken(authenticatedUser.Id, authenticatedUser.UserName,
                                    authenticatedUser.Role?.RoleName!, isRefreshToken: true);

            if (loginRequest.RememberMe)
            {

            }

            return Ok(new LoginResult
            {
                Successful = true,
                Token = token,
                RefreshToken = refreshToken
            });
        }
        catch (KeyNotFoundException ex)
        {
            return Unauthorized(new LoginResult { Successful = false, Error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new LoginResult { Successful = false, Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new LoginResult { Successful = false, Error = $"Có lỗi xảy ra: {ex.Message}" });
        }
    }

    [HttpGet("refresh-token")]
    public async Task<ActionResult<LoginResult>> LoginByRefreshToken(string refreshToken)
    {
        var refreshKey = Configuration.GetValue<string>("Jwt:RefreshKey")!;
        var claims = GetClaimsPrincipalFromToken(refreshToken, refreshKey);
        if (claims == null)
        {
            return Unauthorized(new LoginResult { Successful = false, Error = "Refresh token không hợp lệ" });
        }
        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var user = await UserRepository.GetUserById(int.Parse(userId!));
            var newToken = TokenService.GenerateToken(user.Id, user.UserName, user.Role?.RoleName!,isRefreshToken:false);
            var newRefreshToken = TokenService.GenerateToken(user.Id, user.UserName, user.Role?.RoleName!, isRefreshToken: true);
            return Ok(new LoginResult
            {
                Successful = true,
                Token = newToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
        }
    }

    private ClaimsPrincipal GetClaimsPrincipalFromToken(string refreshToken, string refreshKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(refreshKey);
        var issuer = Configuration.GetValue<string>("Jwt:Issuer")!;
        var audience = Configuration.GetValue<string>("Jwt:Audience")!;

        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out var validatedToken);
            return principal;
        }
        catch
        {
            return null!;
        }
    }
}
