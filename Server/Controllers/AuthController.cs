﻿using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Repositories;
using Shared.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository UserRepository;
    private readonly ITokenService TokenService;

    public AuthController(IUserRepository userRepository, ITokenService tokenService)
    {
        UserRepository = userRepository;
        TokenService = tokenService;
    }

    [HttpPost]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var authenticatedUser = await UserRepository.Authenticate(loginRequest.UserName, loginRequest.Password);
            var token = TokenService.GenerateToken(authenticatedUser.Id, authenticatedUser.UserName);
            if (loginRequest.RememberMe)
            {
                
            }
            return Ok(new LoginResult { Successful = true, Token = token });
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
}