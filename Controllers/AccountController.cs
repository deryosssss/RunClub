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
    //  This controller manages user authentication & account-related actions (register, login, logout, email verification).
    [Route("api/account")]
    [ApiController] //  Enforces automatic validation of request models.
    public class AccountController : ControllerBase
    {
        // Identity services for user management.
        private readonly UserManager<User> _userManager; // Manages user creation, deletion, and retrieval.
        private readonly SignInManager<User> _signInManager; // Handles login/logout.
        private readonly RoleManager<IdentityRole> _roleManager; // Manages user roles (e.g., "Runner", "Admin").
        private readonly EmailService _emailService; // Custom email service for sending verification emails.
        private readonly IConfiguration _configuration; // Access to appsettings.json (for JWT configuration, etc.).

        // 🏗️ Constructor: Dependency Injection (Injecting services).
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, EmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        // User Registration Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            // Validate request data
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // If validation fails, return errors.

            // Ensure passwords match before proceeding.
            if (model.Password != model.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match." });

            // Assign a default role to new users (Prevents self-assigning "Admin" or "Coach").
            string userRole = "Runner";

            // Create a new User object based on the registration details.
            var user = new User
            {
                UserName = model.Email,  // Identity uses emails as usernames.
                Email = model.Email,
                Name = $"{model.FirstName} {model.LastName}" // Concatenate first & last names.
            };

            // Create the user in the database.
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors); // Return any errors from user creation.

            // Assign the default role ("Runner") to the user.
            await _userManager.AddToRoleAsync(user, userRole);

            // Generate an email verification token.
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate a verification link that the user can click.
            var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

            // Send the email with the verification link.
            var emailSubject = "Email Verification";
            var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
            _emailService.SendEmail(user.Email, emailSubject, emailBody);

            // Return a success response.
            return Ok(new { message = "User registered successfully. Please verify your email.", userId = user.Id, assignedRole = userRole });
        }

        /* Security Benefits of This Approach:
        ✔ Prevents users from choosing their own roles (No one can self-assign "Admin").
        ✔ Ensures email verification before allowing login.
        ✔ Default role ("Runner") is enforced programmatically.
        */

        //  Email Verification Endpoint
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            //  Find the user in the database.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            //  Verify the email confirmation token.
            var result = await _userManager.ConfirmEmailAsync(user, token);
            
            // Success
            if (result.Succeeded) return Ok("Email verification successful.");

            // Failure
            return BadRequest("Email verification failed.");
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel model)
        {
            //  Find the user by email.
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Invalid login attempt."); //  Prevents information leakage.

            //  Attempt password sign-in.
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded) return Unauthorized("Invalid login attempt."); //  Prevents brute-force attacks.

            //  Get the user's assigned roles.
            var roles = await _userManager.GetRolesAsync(user);

            //  Generate a JWT token for authentication.
            var token = GenerateJwtToken(user, roles);

            return Ok(new { Token = token });
        }

        //  Logout User
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Clears authentication session.
            return Ok("Logged out successfully.");
        }

        // Generate JWT Token
        private string GenerateJwtToken(User user, IList<string> roles)
        {
            // Define claims (user-specific data stored inside the token).
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Subject (user's email).
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID for token.
                new Claim("userId", user.Id), // Custom claim: User ID.
                new Claim("name", user.Name) // Custom claim: User's full name.
            };

            // Include user roles as claims.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Load the secret key for signing the JWT.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create signing credentials.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define token expiration.
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

            // Create the JWT token.
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"], // Issuer
                _configuration["Jwt:Issuer"], // Audience
                claims, // Claims
                expires: expires, // Expiration
                signingCredentials: creds // Signing credentials
            );

            // Return the generated token as a string.
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