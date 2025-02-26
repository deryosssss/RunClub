using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation($"Fetching users - Page: {pageNumber}, Size: {pageSize}");
            var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            _logger.LogInformation($"Getting user with ID {id}");
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(int roleId)
        {
            _logger.LogInformation($"Fetching users with Role ID {roleId}");
            var users = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            _logger.LogInformation("Creating new user.");
            var createdUser = await _userService.CreateUserAsync(userDto);

            if (createdUser == null)
            {
                _logger.LogWarning("User creation failed due to invalid RoleId.");
                return BadRequest(new { message = "Invalid RoleId" });
            }

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }
    }
}
