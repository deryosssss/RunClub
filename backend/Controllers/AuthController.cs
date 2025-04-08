using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates the user and returns access and refresh tokens.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] LoginDTO loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
                    return BadRequest("Email and password are required.");

                var result = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
                if (result == null)
                    return Unauthorized("Invalid credentials.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the actual exception
                Console.WriteLine("🔥 Exception during login: " + ex.Message);
                return StatusCode(500, "Internal server error during login.");
            }
        }

        /// <summary>
        /// Refreshes JWT token using a valid refresh token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(Problem("Refresh token is required."));

            var result = await _authService.RefreshTokenAsync(request);
            if (result is null)
                return Unauthorized(Problem("Invalid or expired refresh token."));

            return Ok(result);
        }

        /// <summary>
        /// Revokes a user's refresh token (logs out user from all sessions).
        /// </summary>
        [Authorize]
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeTokenRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.UserId))
                return BadRequest(Problem("User ID is required."));

            await _authService.RevokeRefreshTokenAsync(request.UserId);
            return NoContent();
        }
    }
}


/*  Security Features Explained
✔ Prevents Null/Empty Requests – Rejects invalid authentication attempts.
✔ Prevents Token Theft – Only valid refresh tokens can request new access tokens.
✔ Enhances User Experience – Refresh tokens allow seamless re-authentication without frequent logins.
✔ Improves Security – Refresh token revocation ensures users can be forcefully logged out across all sessions.

"The AuthController handles authentication using the IAuthService, which ensures clean separation of concerns."
"The refresh token system enhances security by allowing users to obtain a new access token without re-entering credentials."
"Revoking refresh tokens is essential for logging users out across all devices, preventing unauthorized access." */