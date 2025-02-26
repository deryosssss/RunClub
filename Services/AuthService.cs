using Microsoft.IdentityModel.Tokens;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using RunClubAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RunClubAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly RunClubContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(RunClubContext context, IConfiguration config, ILogger<AuthService> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> LoginAsync(string username, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", username);
                return null;
            }

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO { Token = token, RefreshToken = refreshToken };
        }

        // New Method: Authenticate User
        public async Task<AuthResponseDTO> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid authentication attempt for email: {Email}", email);
                return null;
            }

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO { Token = token, RefreshToken = refreshToken };
        }


        // New Method: Refresh Token
        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid refresh token attempt.");
                return null;
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO { Token = newToken, RefreshToken = newRefreshToken };
        }

        // New Method: Revoke Refresh Token
        public async Task RevokeRefreshTokenAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user != null)
            {
                user.RefreshToken = null;
                await _context.SaveChangesAsync();
            }
        }

        // Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var keyString = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("JWT Key is missing in the configuration.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleClaim = user.Role != null ? user.Role.RoleName : "Unknown"; 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", roleClaim)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate Refresh Token
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", email);
                return false; // Return false if user already exists
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                Role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User")  // assuming the user gets a default role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return true; // Return true if registration is successful
        }

    }
}
