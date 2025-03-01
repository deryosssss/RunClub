using Microsoft.IdentityModel.Tokens; // Provides security algorithms for token generation.
using RunClubAPI.Interfaces; // Interface reference for authentication service abstraction.
using RunClubAPI.Models; // Accesses User and Role models for authentication.
using RunClubAPI.DTOs; // Data transfer objects for structured API responses.
using System.IdentityModel.Tokens.Jwt; // Handles JWT creation and validation.
using System.Security.Claims; // Manages user claims for authentication and authorization.
using System.Security.Cryptography; // Provides cryptographic operations for token security.
using System.Text; // Enables string manipulation for encoding operations.
using Microsoft.EntityFrameworkCore; // Entity Framework Core for database interactions.
using Microsoft.Extensions.Configuration; // Allows access to app settings configuration.
using Microsoft.Extensions.Logging; // Enables logging to track authentication events.
using System.Threading.Tasks; // Supports asynchronous programming.

namespace RunClubAPI.Services // Defines the service layer for authentication logic.
{
    /// <summary>
    /// AuthService handles user authentication, token management, and registration processes.
    /// Implements the IAuthService interface for structured authentication logic.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly RunClubContext _context; // Database context for user-related queries.
        private readonly IConfiguration _config; // Accesses JWT configuration settings.
        private readonly ILogger<AuthService> _logger; // Logs authentication-related events.

        public AuthService(RunClubContext context, IConfiguration config, ILogger<AuthService> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user by verifying credentials and issuing tokens.
        /// </summary>
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

        /// <summary>
        /// Handles user authentication with token generation.
        /// </summary>
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

        /// <summary>
        /// Refreshes an expired access token using a valid refresh token.
        /// </summary>
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

        /// <summary>
        /// Revokes a user's refresh token, preventing further token-based authentication.
        /// </summary>
        public async Task RevokeRefreshTokenAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            if (user != null)
            {
                user.RefreshToken = null;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Generates a JWT token containing user claims.
        /// </summary>
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

        /// <summary>
        /// Generates a secure refresh token using cryptographic functions.
        /// </summary>
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        /// <summary>
        /// Registers a new user, hashes the password, and assigns a default role.
        /// </summary>
        public async Task<bool> RegisterAsync(string email, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", email);
                return false;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                Role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User") // Assigns default role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);
            var refreshToken = GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

/* The AuthService class is responsible for handling user authentication, token generation, and user registration within the RunClubAPI. It follows modern security best practices by using JWT (JSON Web Tokens) for access control and refresh tokens to maintain authentication sessions without requiring repeated logins. The service relies on BCrypt to hash and verify passwords, ensuring password security. Authentication logic is implemented asynchronously using Entity Framework Core, allowing efficient database interactions. Secure token management is achieved through HMAC-SHA256 encryption, using keys stored in the application configuration. The inclusion of logging enhances security by tracking failed login attempts and invalid token usage. Additionally, the service supports refresh token revocation, preventing unauthorized reuse. The repository-based approach ensures a clean separation between business logic and data access, making the system more modular, testable, and scalable. */