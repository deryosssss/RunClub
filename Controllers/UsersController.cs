using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Interfaces;
using RunClub.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.Models;

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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsersAsync();
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
