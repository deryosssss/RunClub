using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
// This controller handles authentication-related actions: login, refresh token, and token revocation.
    [Route("api/[controller]")] // Base route: "api/auth"
    [ApiController] // Enforces automatic model validation & request binding.
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // Service to handle authentication logic.

        // Constructor: Dependency Injection
        public AuthController(IAuthService authService)
        {
            _authService = authService; // Injects the authentication service.
        }

        // Authenticate User (Login)
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponseDTO>> AuthenticateUser([FromBody] LoginDTO loginRequest)
        {
            // Validate the request
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid credentials"); // Prevents null/empty requests.
            }

            // Authenticate the user using the service
            var authResponse = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);

            if (authResponse == null)
            {
                return Unauthorized("Invalid email or password"); // Prevents information leakage.
            }

            return Ok(authResponse); // Returns JWT & refresh token if successful.
        }

        // Refresh Token (Used to get a new access token without logging in again)
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate request
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Invalid refresh token"); // Prevents null/empty requests.
            }

            // Attempt to refresh the token
            var authResponse = await _authService.RefreshTokenAsync(request);

            if (authResponse == null)
            {
                return Unauthorized("Invalid refresh token"); // Prevents token abuse.
            }

            return Ok(authResponse); // Returns new JWT & refresh token.
        }

        // Revoke Refresh Token (Logs out user across all devices)
        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeTokenRequest request)
        {
            // Validate request
            if (request == null || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("Invalid user ID"); // Prevents null/empty requests.
            }

            //  Revoke the user's refresh token (logout from all sessions)
            await _authService.RevokeRefreshTokenAsync(request.UserId);
            
            return NoContent(); // Indicates successful logout (204 No Content).
        }
    }
}

/* 🛡️ Security Features Explained
✔ Prevents Null/Empty Requests – Rejects invalid authentication attempts.
✔ Prevents Token Theft – Only valid refresh tokens can request new access tokens.
✔ Enhances User Experience – Refresh tokens allow seamless re-authentication without frequent logins.
✔ Improves Security – Refresh token revocation ensures users can be forcefully logged out across all sessions.

✅ What You Can Say in Your Viva
"The AuthController handles authentication using the IAuthService, which ensures clean separation of concerns."
"The refresh token system enhances security by allowing users to obtain a new access token without re-entering credentials."
"Revoking refresh tokens is essential for logging users out across all devices, preventing unauthorized access." */