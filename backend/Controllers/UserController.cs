using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")] // Optional protection
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Fetching users - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            _logger.LogInformation("Getting user with ID {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        // GET: api/users/role/{role}
        [HttpGet("role/{role}")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(string role)
        {
            _logger.LogInformation("Fetching users by role: {Role}", role);
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        // POST: api/users
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDTO>> PostUser([FromBody] CreateUserDTO createUserDto)
        {
            _logger.LogInformation("Creating new user");

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
                return BadRequest(new { message = "Password is required." });

            var userDto = new UserDTO
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                RoleId = createUserDto.RoleId ?? "Runner" // Default to Runner if none provided
            };

            var createdUser = await _userService.CreateUserAsync(userDto, createUserDto.Password);

            if (createdUser == null)
            {
                _logger.LogWarning("User creation failed.");
                return BadRequest(new { message = "User creation failed" });
            }

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Attempting to delete user with ID {UserId}", id);
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            var success = await _userService.DeleteUserAsync(id);
            return success ? NoContent() : BadRequest(new { message = "Failed to delete user." });
        }
    }
}

/* The UsersController in my ASP.NET Core Web API is responsible for handling user-related operations such as fetching users, retrieving specific users, filtering users based on roles, and creating new users with assigned roles. It follows a structured, service-oriented approach using dependency injection to interact with the IUserService, which encapsulates business logic, ensuring modularity and maintainability. The controller supports pagination for user retrieval to optimize performance when handling large datasets. Additionally, it implements logging using ILogger to track important actions, such as retrieving or creating users, which aids in debugging and monitoring. Each endpoint includes proper validation, ensuring that errors such as missing users or failed user creation are handled gracefully. The use of RESTful best practices (e.g., returning 200 OK, 404 Not Found, and 201 Created responses) enhances API usability. By implementing role-based filtering, this controller allows administrators to efficiently manage users based on their roles, ensuring a secure and well-organized system.*/