using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Shared.Models;
using Shared.Repositories;
using System.Security.Claims;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServerRepository UserRepository;
        private readonly IHubContext<NotificationHub> HubContext;

        public UserController(IUserServerRepository userRepository, IHubContext<NotificationHub> hubContext)
        {
            UserRepository = userRepository;
            HubContext = hubContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await UserRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResult>> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterResult { 
                    Successful = false, 
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList() });
            }

            try
            {
                User user = new User
                {
                    UserName = registerRequest.UserName,
                    Email = registerRequest.Email,
                    Password = registerRequest.Password,
                    RoleId = registerRequest.RoleId,
                };

                await UserRepository.AddUser(user);
                return Ok(new RegisterResult { Successful = true });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new RegisterResult { Successful = false, Errors = new List<string> { ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RegisterResult { Successful = false, Errors = new List<string> { $"Có lỗi xảy ra: {ex.Message}" } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var isAdmin = User.IsInRole("Admin");

                if (!isAdmin && userIdFromToken != id)
                {
                    return Unauthorized("Bạn không có quyền truy cập thông tin của người dùng khác.");
                }

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
                var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var isAdmin = User.IsInRole("Admin");

                if (!isAdmin && userIdFromToken != id)
                {
                    return Unauthorized("Bạn không có quyền cập nhật thông tin của người dùng khác.");
                }

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
        [Authorize(Roles = "Admin")]
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

        [HttpPut("{id}/change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userIdFromToken != id)
            {
                return Unauthorized("Bạn không có quyền thay đổi mật khẩu của người dùng khác.");
            }

            try
            {
                await UserRepository.ChangePassword(id, changePasswordRequest);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        [HttpPut("{id}/set-status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SetUserStatus(int id, [FromBody] bool IsActive)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await UserRepository.SetUserStatus(id, IsActive);
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

        [HttpPut("{id}/change-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeRole(int id, [FromBody] int roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await UserRepository.ChangeRole(id, roleId);
                await HubContext.Clients.All.SendAsync("RoleChanged");
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
