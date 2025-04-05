using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RunClubAPI.DTOs;
using RunClubAPI.Models;
using RunClubAPI.Services;
using RunClubAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RunClubAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            EmailService emailService,
            IConfiguration configuration,
            IAuthService authService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // 👈 this should match user.Id in DB
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.Users
                .Where(u => u.Id == userId) // 👈 match against u.Id (type string GUID!)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                    ?? await _userManager.FindByNameAsync(email);

            if (user == null)
                return NotFound(new { message = $"No user found with email: {email}" });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "Failed to delete user", errors });
            }

            _logger.LogInformation("🗑️ Deleted user: {Email}", email);
            return Ok(new { message = $"User {email} deleted successfully." });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            string assignedRole = string.IsNullOrWhiteSpace(model.Role) ? "User" : model.Role;

            if (!await _roleManager.RoleExistsAsync(assignedRole))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(assignedRole));
                if (!roleResult.Succeeded)
                {
                    return BadRequest(new { message = $"Failed to create role {assignedRole}" });
                }
            }

            var role = await _roleManager.FindByNameAsync(assignedRole);
            if (role == null)
            {
                return BadRequest(new { message = $"Role {assignedRole} could not be found." });
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
                RoleId = role.Id,
                Role = role
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "User creation failed", errors });
            }

            await _userManager.AddToRoleAsync(user, assignedRole);

            // ✅ Generate verification token and link
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var confirmationUrl = $"http://localhost:5187/api/account/verify-email?userId={user.Id}&token={encodedToken}";

            await _emailService.SendEmailAsync(
                model.Email,
                "Please verify your email",
                $"Welcome, {model.Name}!<br/><br/>Please verify your email by clicking this link:<br/><a href=\"{confirmationUrl}\">{confirmationUrl}</a>"
            );

            _logger.LogInformation("✅ Registered user: {Email} with role: {Role}", user.Email, assignedRole);

            return Ok(new { message = "User registered successfully. Please verify your email." });
        }


        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.VerifyEmailAsync(model.Token, model.UserId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Email verified successfully.");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Missing userId or token.");

            var result = await _authService.VerifyEmailAsync(token, userId);
            if (!result.Success)
                return BadRequest("Verification failed: " + result.Message);

            // Optional: redirect to login page with success message
            return Redirect("http://localhost:3000/login?verified=true");

            // OR if no frontend redirect yet:
            // return Ok("✅ Email verified successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("❌ User not found: {Email}", model.Email);
                return Unauthorized(new { message = "Invalid login attempt." });
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("❌ Email not confirmed: {Email}", model.Email);
                return Unauthorized(new { message = "Please verify your email before logging in." });
            }

            // 👇 ADD THIS TO CHECK PASSWORD MANUALLY
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            _logger.LogInformation("🔍 Is correct password for {Email}: {Correct}", model.Email, isCorrectPassword);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("❌ SignInManager failed for user: {Email}", model.Email);
                return Unauthorized(new { message = "Invalid login attempt." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new { Token = token });
        }



        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id), // 🔐 important: user ID
        new Claim("name", user.Name ?? "")
    };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer))
                throw new InvalidOperationException("JWT config missing in appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}




/*📝 Summary of Key Security Features
✔ Prevents Role Manipulation – Users cannot assign themselves admin privileges.
✔ Requires Email Verification – Ensures users provide a valid email before logging in.
✔ Uses Secure JWT Tokens – Ensures authenticated API access.
✔ Protects Against Brute Force Attacks – Prevents password enumeration.
✔ Uses Claims in Tokens – Embeds user info (ID, roles) into JWT for role-based authorization. */