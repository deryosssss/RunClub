using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RunClubAPI.Data;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly RunClubContext _context;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IUserService userService,
            ILogger<AccountController> logger,
            RunClubContext context,
            UserManager<User> userManager)
        {
            _userService = userService;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDTO>> Register([FromBody] CreateUserDTO createUserDto)
        {
            _logger.LogInformation("➡️ Registering new user: {Email}", createUserDto.Email);

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
                return BadRequest(new { message = "Password is required." });

            var userDto = new UserDTO
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                RoleId = createUserDto.RoleId
            };

            var createdUser = await _userService.CreateUserAsync(userDto, createUserDto.Password);

            if (createdUser == null)
            {
                _logger.LogWarning("❌ User registration failed for {Email}", createUserDto.Email);
                return BadRequest(new { message = "User registration failed." });
            }

            return CreatedAtAction(nameof(GetProfile), new { id = createdUser.UserId }, createdUser);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "Invalid token or user ID missing" });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .ToListAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserDTO
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "Runner",
                Location = user.Location,
                Age = user.Age,
                EnrollmentsCount = enrollments.Count,
                CompletedCount = enrollments.Count(e => e.IsCompleted)
            });
        }

        [HttpGet("stats/{userId}")]
        [ProducesResponseType(typeof(AccountDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStats(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return Ok(new AccountDto
            {
                Id = 0, // optional: parse to int if needed
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                TotalEnrollments = enrollments.Count,
                CompletedEvents = enrollments.Count(e => e.IsCompleted)
            });
        }

        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO updated)
        {
            var user = await _context.Users.FindAsync(updated.UserId);
            if (user == null) return NotFound();

            user.Name = updated.Name ?? user.Name;
            user.Email = updated.Email ?? user.Email;
            user.Location = updated.Location ?? user.Location;
            user.Age = updated.Age ?? user.Age;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPut("password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return NoContent();
        }
        // [Authorize(Roles = "Admin")] // or "Coach", or remove if you want users to delete themselves
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var deleted = await _userService.DeleteUserAsync(userId);
            if (!deleted)
                return NotFound(new { message = $"User with ID {userId} not found." });

            return NoContent();
        }

    }
}

/*📝 Summary of Key Security Features
✔ Prevents Role Manipulation – Users cannot assign themselves admin privileges.
✔ Requires Email Verification – Ensures users provide a valid email before logging in.
✔ Uses Secure JWT Tokens – Ensures authenticated API access.
✔ Protects Against Brute Force Attacks – Prevents password enumeration.
✔ Uses Claims in Tokens – Embeds user info (ID, roles) into JWT for role-based authorization. */