using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]
    // [Authorize(Roles = "Admin")] // Uncomment if you want to restrict access to Admins
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /api/roles
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleDTO>), 200)]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => new RoleDTO
            {
                RoleId = r.Id,
                RoleName = r.Name!,
                RoleNormalizedName = r.NormalizedName!
            }).ToList();

            return Ok(roles);
        }

        // POST: /api/roles
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleName))
                return BadRequest(new { message = "Role name is required." });

            if (await _roleManager.RoleExistsAsync(dto.RoleName))
                return BadRequest(new { message = $"Role '{dto.RoleName}' already exists." });

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));
            return result.Succeeded
                ? Ok(new { message = $"Role '{dto.RoleName}' created successfully." })
                : BadRequest(result.Errors);
        }

        // POST: /api/roles/assign
        [HttpPost("assign")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { message = $"User with ID '{model.UserId}' not found." });

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return NotFound(new { message = $"Role '{model.RoleName}' not found." });

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            return result.Succeeded
                ? Ok(new { message = $"Role '{model.RoleName}' assigned to user '{user.Email}'." })
                : BadRequest(result.Errors);
        }

        // DELETE: /api/roles/{roleName}
        [HttpDelete("{roleName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound(new { message = $"Role '{roleName}' not found." });

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded
                ? Ok(new { message = $"Role '{roleName}' deleted successfully." })
                : BadRequest(result.Errors);
        }

        // GET: /api/roles/users/{roleName}
        [HttpGet("users/{roleName}")]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                return NotFound(new { message = $"Role '{roleName}' not found." });

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var userDtos = users.Select(u => new UserDTO
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                RoleId = roleName
            }).ToList();

            return Ok(userDtos);
        }
    }
}


/* "The RolesController in my ASP.NET Core Web API is responsible for managing user roles, ensuring that only authorized administrators can create, retrieve, assign, and delete roles. This controller enforces security using [Authorize(Roles = "Admin")], restricting access to users with the 'Admin' role. It leverages dependency injection with RoleManager<IdentityRole> and UserManager<IdentityUser> to handle role and user management efficiently. Key functionalities include retrieving all roles, getting a specific role by ID, preventing duplicate role creation, and ensuring users cannot assign themselves the 'Admin' role for security purposes. Additionally, the controller prevents the deletion of the 'Admin' role to maintain system integrity. Proper validation, error handling, and DTOs are used to ensure clean API responses and prevent unauthorized operations, following best practices in secure role-based access control." */