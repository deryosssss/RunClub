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

    // User Registration
    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == username))
        {
            _logger.LogWarning("Registration failed. Email already in use: {Email}", username);
            return false;
        }

        var user = new User
        {
            Email = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = 2 // Assuming "User" role has an ID of 2 in the database
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User registered successfully: {Email}", username);
        return true;
    }

    // User Login
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

    // Refresh Token Logic
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

    // Revoke Refresh Token
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role.Name)
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
}

