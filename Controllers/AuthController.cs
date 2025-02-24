using Microsoft.AspNetCore.Mvc;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using RunClub.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var result = await _authService.AuthenticateUserAsync(loginDto);
        if (result == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenRequest);
        if (result == null)
        {
            return Unauthorized("Invalid refresh token.");
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return BadRequest("User ID not found in token.");

        await _authService.RevokeRefreshTokenAsync(userId);
        return Ok("Logged out successfully.");
    }
}
