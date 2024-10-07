using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Repositories;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository UserRepository;
        private readonly ITokenService TokenService;

        public UserController(IUserRepository userRepository, ITokenService tokenService)
        {
            UserRepository = userRepository;
            TokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await UserRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await UserRepository.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var authenticatedUser = await UserRepository.Authenticate(loginRequest.UserName, loginRequest.Password);
                var token = TokenService.GenerateToken(authenticatedUser.Id, authenticatedUser.UserName);
                return Ok(new { Token = token });
            }
            catch (KeyNotFoundException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await UserRepository.GetUserById(id);
                return Ok(user);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("ID trong URL không khớp với ID của người dùng.");
            }

            try
            {
                await UserRepository.UpdateUser(user);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await UserRepository.DeleteUser(id);
                return NoContent();
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
    }
}
