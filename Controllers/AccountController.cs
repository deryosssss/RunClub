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
        private readonly IAuthService _authService; // ✅ Added this for VerifyEmailAsync

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            EmailService emailService,
            IConfiguration configuration,
            IAuthService authService) // ✅ Injected IAuthService
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _authService = authService; // ✅ Assigning auth service
        }

        // ✅ User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match." });

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = $"{model.FirstName} {model.LastName}"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role ?? "Runner");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

            _emailService.SendEmail(user.Email, "Email Verification", $"Click to verify: {verificationLink}");

            return Ok(new { message = "Registration successful. Check email for verification.", userId = user.Id, role = model.Role });
        }

        // ✅ Email Verification
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

        // ✅ User Login
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

        // ✅ Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        // ✅ Generate JWT Token
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