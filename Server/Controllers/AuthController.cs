using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(SignInManager<IdentityUser> signInManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService; 
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, false);

            if (!result.Succeeded)
                return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            var token = _tokenService.GenerateToken(loginRequest.UserName);

            return Ok(new LoginResult { Successful = true, Token = token });
        }
    }
}
