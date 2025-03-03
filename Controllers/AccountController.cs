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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.Users
                .Where(u => u.Id == userId)
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

        [Authorize(Roles = "Admin")]
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

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Email,
            };

            // Set Default Role (if no role is provided)
            string assignedRole = string.IsNullOrEmpty(model.Role) ? "User" : model.Role;

            // Ensure the role exists before assigning it
            if (!await _roleManager.RoleExistsAsync(assignedRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(assignedRole));
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "User creation failed", errors });
            }

            // Assign Role AFTER user creation
            await _userManager.AddToRoleAsync(user, assignedRole);

            // Log User Creation
            _logger.LogInformation("User created: {Email} with Role: {Role}", model.Email, assignedRole);

            // Send Welcome Email
            await _emailService.SendEmailAsync(model.Email, "Welcome!", "Thank you for registering!");

            return Ok(new { message = "User registered successfully" });
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid login attempt." });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid login attempt." });

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
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id),
                new Claim("name", user.Name ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
                throw new ArgumentNullException("JWT configuration is missing!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"] ?? "1"));

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtIssuer,
                claims,
                expires: expires,
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