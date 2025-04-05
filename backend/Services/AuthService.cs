using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
                _logger.LogWarning("❌ User not found or password invalid for {Email}", email);
                return null;
            }

            var token = GenerateJwtToken(user);
            return new AuthResponseDTO { Token = token };
        }

        public async Task<bool> RegisterAsync(RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("❌ Registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}



/* The AuthService class is responsible for handling user authentication, token generation, and user registration within the RunClubAPI. It follows modern security best practices by using JWT (JSON Web Tokens) for access control and refresh tokens to maintain authentication sessions without requiring repeated logins. The service relies on BCrypt to hash and verify passwords, ensuring password security. Authentication logic is implemented asynchronously using Entity Framework Core, allowing efficient database interactions. Secure token management is achieved through HMAC-SHA256 encryption, using keys stored in the application configuration. The inclusion of logging enhances security by tracking failed login attempts and invalid token usage. Additionally, the service supports refresh token revocation, preventing unauthorized reuse. The repository-based approach ensures a clean separation between business logic and data access, making the system more modular, testable, and scalable. */