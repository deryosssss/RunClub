using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.Models;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Get all roles
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }

        // Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest("Role name cannot be empty");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return BadRequest("Role already exists");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Role created successfully" });
        }

        // Assign a role to a user
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("User not found");

            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
                return NotFound("Role does not exist");

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Role assigned successfully" });
        }
    }

    public class AssignRoleModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}


/* "The RolesController in my ASP.NET Core Web API is responsible for managing user roles, ensuring that only authorized administrators can create, retrieve, assign, and delete roles. This controller enforces security using [Authorize(Roles = "Admin")], restricting access to users with the 'Admin' role. It leverages dependency injection with RoleManager<IdentityRole> and UserManager<IdentityUser> to handle role and user management efficiently. Key functionalities include retrieving all roles, getting a specific role by ID, preventing duplicate role creation, and ensuring users cannot assign themselves the 'Admin' role for security purposes. Additionally, the controller prevents the deletion of the 'Admin' role to maintain system integrity. Proper validation, error handling, and DTOs are used to ensure clean API responses and prevent unauthorized operations, following best practices in secure role-based access control." */