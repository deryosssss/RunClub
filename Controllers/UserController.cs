using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    // This controller handles user-related operations such as retrieving, creating, and filtering users.
    [Route("api/[controller]")] // Defines the API route as "api/Users"
    [ApiController] // Specifies that this is an API controller (automatically handles HTTP 400 responses)
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Dependency injection for user-related services
        private readonly ILogger<UsersController> _logger; // Logger for tracking application behavior

        // Constructor to inject dependencies (UserService and Logger)
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/Users
        // Retrieves a paginated list of users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation($"Fetching users - Page: {pageNumber}, Size: {pageSize}");

            // Fetch users with pagination support (to optimize large datasets)
            var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return Ok(users); // Return users in JSON format with HTTP 200 OK
        }

        // GET: api/Users/{id}
        // Retrieves a specific user by their unique ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            _logger.LogInformation($"Getting user with ID {id}");

            // Fetch user details by ID
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null) // Handle case where user is not found
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound(); // Return 404 Not Found response
            }

            return Ok(user); // Return user details with HTTP 200 OK
        }

        // GET: api/Users/role/{roleId}
        // Retrieves a list of users based on their role
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(string roleId)
        {
            _logger.LogInformation($"Fetching users with Role ID {roleId}");

            // Fetch users who belong to a specific role
            var users = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(users); // Return filtered users list
        }

        // POST: api/Users
        // Creates a new user, ensuring role assignment
// POST: api/Users
// Creates a new user, ensuring role assignment
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDto)
        {
            _logger.LogInformation("Creating new user with role information.");

            if (string.IsNullOrEmpty(createUserDto.Password))
            {
                return BadRequest(new { message = "Password is required." });
            }

            // Convert CreateUserDTO to UserDTO
            var userDto = new UserDTO
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                RoleId = createUserDto.RoleId ?? "1" // Default to "runner"
            };

            // Call CreateUserAsync and pass the required password argument
            var createdUser = await _userService.CreateUserAsync(userDto, createUserDto.Password);

            if (createdUser == null)
            {
                _logger.LogWarning("User creation failed.");
                return BadRequest(new { message = "User creation failed" });
            }

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }
    }
}
/* The UsersController in my ASP.NET Core Web API is responsible for handling user-related operations such as fetching users, retrieving specific users, filtering users based on roles, and creating new users with assigned roles. It follows a structured, service-oriented approach using dependency injection to interact with the IUserService, which encapsulates business logic, ensuring modularity and maintainability. The controller supports pagination for user retrieval to optimize performance when handling large datasets. Additionally, it implements logging using ILogger to track important actions, such as retrieving or creating users, which aids in debugging and monitoring. Each endpoint includes proper validation, ensuring that errors such as missing users or failed user creation are handled gracefully. The use of RESTful best practices (e.g., returning 200 OK, 404 Not Found, and 201 Created responses) enhances API usability. By implementing role-based filtering, this controller allows administrators to efficiently manage users based on their roles, ensuring a secure and well-organized system.*/