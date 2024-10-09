using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static User LoggedOutUser = new User { IsAuthenticated = false };

        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterRequest registerRequest)
        {
            var newUser = new IdentityUser { UserName = registerRequest.UserName, Email = registerRequest.Email };

            var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }
            return Ok(new RegisterResult { Successful = true });
        }
    }
}
