using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RunClubAPI.DTOs;
using RunClubAPI.Models;
using RunClubAPI.Services;
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

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, EmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match." });

            // Automatically assign the default role ("Runner") – users cannot select their own role
            string userRole = "Runner";

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = $"{model.FirstName} {model.LastName}"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign default role
            await _userManager.AddToRoleAsync(user, userRole);

            // Generate an email verification token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

            // Send the verification email
            var emailSubject = "Email Verification";
            var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
            _emailService.SendEmail(user.Email, emailSubject, emailBody);

            return Ok(new { message = "User registered successfully. Please verify your email.", userId = user.Id, assignedRole = userRole });
        }

/*🔒 How This Improves Security
✔ Prevents users from selecting their own role (No one can self-assign "Coach" or "Admin").
✔ Ensures every new user starts as a "Runner" by default.
✔ Special roles (like "Coach") can only be assigned manually by an Admin.

✅ How to Assign a "Coach" Role? (Admin Only)
If an admin needs to promote a user to "Coach", you should have a separate API endpoint, something like: */

        // ✅ Email Verification Endpoint
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded) return Ok("Email verification successful.");
            return BadRequest("Email verification failed.");
        }

        // ✅ User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Invalid login attempt.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded) return Unauthorized("Invalid login attempt.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            return Ok(new { Token = token });
        }

        // ✅ Logout User
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out successfully.");
        }

        // ✅ Generate JWT Token
        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id),
                new Claim("name", user.Name)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

// // using RunClub.Models;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Security.Claims;
// using System.Text;
// using System.Threading.Tasks;
// using RunClubAPI.Models;
// using RunClubAPI.Services;

// namespace RunClubAPI.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AccountController : ControllerBase
//     {
//         private readonly UserManager<IdentityUser> _userManager;
//         private readonly SignInManager<IdentityUser> _signInManager;
//         private readonly EmailService _emailService;
//         private readonly IConfiguration _configuration;

//         public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService, IConfiguration configuration)
//         {
//             _userManager = userManager;
//             _signInManager = signInManager;
//             _emailService = emailService;
//             _configuration = configuration;
//         }

//         [HttpPost("register")]
//         public async Task<IActionResult> Register(AuthModel model)
//         {
//             var user = new IdentityUser { UserName = model.Email, Email = model.Email };
//             var result = await _userManager.CreateAsync(user, model.Password);

//             if (result.Succeeded)
//             {
//                 // Generate an email verification token
//                 var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

//                 // Create the verification link
//                 var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

//                 // Send the verification email
//                 var emailSubject = "Email Verification";
//                 var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
//                 _emailService.SendEmail(user.Email, emailSubject, emailBody);

//                 return Ok("User registered successfully. An email verification link has been sent.");
//             }

//             return BadRequest(result.Errors);
//         }


//         // Add an action to handle email verification
//         [HttpGet("verify-email")]
//         public async Task<IActionResult> VerifyEmail(string userId, string token)
//         {
//             var user = await _userManager.FindByIdAsync(userId);

//             if (user == null)
//             {
//                 return NotFound("User not found.");
//             }

//             var result = await _userManager.ConfirmEmailAsync(user, token);

//             if (result.Succeeded)
//             {
//                 return Ok("Email verification successful.");
//             }

//             return BadRequest("Email verification failed.");
//         }



//         [HttpPost("login")]
//         public async Task<IActionResult> Login(AuthModel model)
//         {
//             var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

//             if (result.Succeeded)
//             {
//                 var user = await _userManager.FindByEmailAsync(model.Email);
//                 var roles = await _userManager.GetRolesAsync(user);
//                 var token = GenerateJwtToken(user, roles);
//                 return Ok(new { Token = token });
//             }

//             return Unauthorized("Invalid login attempt.");
//         }

//         [HttpPost("logout")]
//         public async Task<IActionResult> Logout()
//         {
//             await _signInManager.SignOutAsync();
//             return Ok("Logged out");
//         }
//         private string GenerateJwtToken(IdentityUser user, IList<string> roles)
//         {
//             var claims = new List<Claim>
//             {
//                 new Claim(JwtRegisteredClaimNames.Sub, user.Email),
//                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//             };

//             // Add roles as claims
//             foreach (var role in roles)
//             {
//                 claims.Add(new Claim(ClaimTypes.Role, role));
//             }

//             var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//             var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

//             var token = new JwtSecurityToken(
//                 _configuration["Jwt:Issuer"],
//                 _configuration["Jwt:Issuer"],
//                 claims,
//                 expires: expires,
//                 signingCredentials: creds
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }

//     }

// }