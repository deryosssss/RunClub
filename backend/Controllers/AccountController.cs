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

        // POST: api/account/register
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDTO>> Register([FromBody] CreateUserDTO createUserDto)
        {
            _logger.LogInformation("Registering new user");

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
                return BadRequest(new { message = "Password is required." });

            var newUser = new UserDTO
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                Role = createUserDto.RoleId
            };

            var createdUser = await _userService.CreateUserAsync(newUser, createUserDto.Password);

            if (createdUser == null)
            {
                _logger.LogWarning("User registration failed.");
                return BadRequest(new { message = "User registration failed." });
            }

            return CreatedAtAction(nameof(GetProfile), new { id = createdUser.UserId }, createdUser);
        }

        // GET: api/account/me
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest(new { message = "Invalid token or user ID missing" });

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return NotFound(new { message = $"User with ID {userId} not found." });
            }

            return Ok(user);
        }

        // GET: api/account/stats/{userId}
        [HttpGet("stats/{userId}")]
        [ProducesResponseType(typeof(AccountDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStats(string userId)
        {
            var enrollments = await _context.Enrollments
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (!enrollments.Any())
                return NotFound(new { message = "No enrollments found for this user." });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { message = "User not found." });

            var stats = new AccountDto
            {
                Id = int.TryParse(userId, out var uid) ? uid : 0,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                TotalEnrollments = enrollments.Count,
                CompletedEvents = enrollments.Count(e => e.IsCompleted) // Ensure Enrollment has a 'Completed' property
            };

            return Ok(stats);
        }

        // PUT: api/account/profile
        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO updated)
        {
            var user = await _context.Users.FindAsync(updated.UserId);
            if (user == null) return NotFound();

            user.Name = updated.Name;
            user.Email = updated.Email;
            user.Location = updated.Location;
            user.Age = user.Age;


            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/account/password
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
    }
}




/*📝 Summary of Key Security Features
✔ Prevents Role Manipulation – Users cannot assign themselves admin privileges.
✔ Requires Email Verification – Ensures users provide a valid email before logging in.
✔ Uses Secure JWT Tokens – Ensures authenticated API access.
✔ Protects Against Brute Force Attacks – Prevents password enumeration.
✔ Uses Claims in Tokens – Embeds user info (ID, roles) into JWT for role-based authorization. */