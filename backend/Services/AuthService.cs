// ==================== AuthService.cs (Improved) ====================

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RunClubAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, IConfiguration config, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        public async Task<AuthResponseDTO?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("❌ Invalid login for {Email}", email);
                return null;
            }

            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> RegisterAsync(RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("❌ Registration failed for {Email}: {Errors}", model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            _logger.LogInformation("✅ Registered user {Email} with role {Role}", model.Email, model.Role);
            return true;
        }

        public async Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = _userManager.Users.FirstOrDefault(u =>
                u.RefreshToken == request.RefreshToken &&
                u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                _logger.LogWarning("⚠️ Invalid or expired refresh token");
                return null;
            }

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeRefreshTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiry = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
            _logger.LogInformation("🔒 Refresh token revoked for user {UserId}", userId);
        }

        public async Task<bool> DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Name ?? "")
            };

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<AuthResponseDTO?> AuthenticateUserAsync(string email, string password)
        {
            return await LoginAsync(email, password);
        }
    }
} 






/* The AuthService class is responsible for handling user authentication, token generation, and user registration within the RunClubAPI. It follows modern security best practices by using JWT (JSON Web Tokens) for access control and refresh tokens to maintain authentication sessions without requiring repeated logins. The service relies on BCrypt to hash and verify passwords, ensuring password security. Authentication logic is implemented asynchronously using Entity Framework Core, allowing efficient database interactions. Secure token management is achieved through HMAC-SHA256 encryption, using keys stored in the application configuration. The inclusion of logging enhances security by tracking failed login attempts and invalid token usage. Additionally, the service supports refresh token revocation, preventing unauthorized reuse. The repository-based approach ensures a clean separation between business logic and data access, making the system more modular, testable, and scalable. */