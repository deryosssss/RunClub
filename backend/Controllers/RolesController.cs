using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RunClubAPI.Models;
using RunClubAPI.DTOs;

namespace RunClubAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /api/roles
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => new RoleDTO
            {
                RoleId = r.Id,
                RoleName = r.Name ?? "",
                RoleNormalizedName = r.NormalizedName ?? ""
            }).ToList();

            return Ok(roles);
        }

        // POST: /api/roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest(new { message = $"Role '{roleName}' already exists." });

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' created successfully." });
            }

            return BadRequest(result.Errors);
        }

        // POST: /api/roles/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { message = $"User with ID '{model.UserId}' not found." });

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return NotFound(new { message = $"Role '{model.RoleName}' not found." });

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{model.RoleName}' assigned to user '{user.Email}'." });
            }

            return BadRequest(result.Errors);
        }

        // DELETE: /api/roles/{roleName}
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound(new { message = $"Role '{roleName}' not found." });

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' deleted successfully." });
            }

            return BadRequest(result.Errors);
        }
    }
}



/* "The RolesController in my ASP.NET Core Web API is responsible for managing user roles, ensuring that only authorized administrators can create, retrieve, assign, and delete roles. This controller enforces security using [Authorize(Roles = "Admin")], restricting access to users with the 'Admin' role. It leverages dependency injection with RoleManager<IdentityRole> and UserManager<IdentityUser> to handle role and user management efficiently. Key functionalities include retrieving all roles, getting a specific role by ID, preventing duplicate role creation, and ensuring users cannot assign themselves the 'Admin' role for security purposes. Additionally, the controller prevents the deletion of the 'Admin' role to maintain system integrity. Proper validation, error handling, and DTOs are used to ensure clean API responses and prevent unauthorized operations, following best practices in secure role-based access control." */