using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using System.Threading.Tasks;

namespace RunClub.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO request)
    {
        var success = await _authService.RegisterAsync(request.Email, request.Password);
        if (success)
            return Ok(new { message = "User registered successfully." });

        return BadRequest(new { message = "Registration failed. Email may already exist." });
    }

    /// <summary>
    /// Authenticate user and return JWT & Refresh Token
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var result = await _authService.AuthenticateUserAsync(loginDto);
        if (result == null)
            return Unauthorized(new { message = "Invalid credentials." });

        return Ok(result);
    }

    /// <summary>
    /// Refresh JWT Token using a valid Refresh Token
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenRequest);
        if (result == null)
            return Unauthorized(new { message = "Invalid or expired refresh token." });

        return Ok(result);
    }

    /// <summary>
    /// Logout user (Revoke Refresh Token)
    /// Requires Authorization
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userId == null)
            return BadRequest(new { message = "User ID not found in token." });

        await _authService.RevokeRefreshTokenAsync(userId);
        return Ok(new { message = "Logged out successfully." });
    }
}

}