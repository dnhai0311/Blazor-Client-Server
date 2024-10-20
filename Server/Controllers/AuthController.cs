using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Repositories;
using Shared.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserServerRepository UserRepository;
    private readonly ITokenService TokenService;

    public AuthController(IUserServerRepository userRepository, ITokenService tokenService)
    {
        UserRepository = userRepository;
        TokenService = tokenService;
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

            await TokenService.StoreRefreshToken(authenticatedUser.Id, refreshToken, DateTime.UtcNow.AddDays(TokenService.RefreshTokenExpiry));

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
        if (!await TokenService.ValidateRefreshToken(refreshToken))
        {
            return Unauthorized(new LoginResult { Successful = false, Error = "Refresh token không hợp lệ" });
        }

        var claims = GetClaimsPrincipalFromToken(refreshToken);
        if (claims == null)
        {
            return Unauthorized(new LoginResult { Successful = false, Error = "Refresh token không hợp lệ" });
        }

        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var user = await UserRepository.GetUserById(int.Parse(userId!));
            var newToken = TokenService.GenerateToken(user.Id, user.UserName, user.Role?.RoleName!, isRefreshToken: false);
            var newRefreshToken = TokenService.GenerateToken(user.Id, user.UserName, user.Role?.RoleName!, isRefreshToken: true);

            await TokenService.StoreRefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(TokenService.RefreshTokenExpiry));

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

    private ClaimsPrincipal GetClaimsPrincipalFromToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var refreshKey = TokenService.RefreshKey; 
        var issuer = TokenService.Issuer;
        var audience = TokenService.Audience;

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshKey))
            }, out var validatedToken);
            return principal;
        }
        catch
        {
            return null!;
        }
    }
}
