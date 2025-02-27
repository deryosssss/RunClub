using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunClubAPI.DTOs;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only Admins can manage roles
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // ✅ Get all roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(new { message = "Role not found." });
            }

            return Ok(new RoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name,
                RoleNormalizedName = role.NormalizedName
            });
        }


        // ✅ Create a new role (Prevents duplicate roles)
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("Role already exists.");
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }

            return BadRequest(result.Errors);
        }

        // ✅ Assign a role to a user (Prevents assigning "Admin" role to oneself)
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return NotFound("Role not found.");
            }

            // Prevent users from assigning themselves the "Admin" role
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == user.Id && model.RoleName == "Admin")
            {
                return BadRequest("You cannot assign yourself the Admin role.");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok("Role assigned successfully.");
            }

            return BadRequest(result.Errors);
        }

        // ✅ Delete a role (Prevents deleting "Admin" role)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            // Prevent deletion of the "Admin" role
            if (role.Name == "Admin")
            {
                return BadRequest("The Admin role cannot be deleted.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
    }

    // ✅ Model for assigning roles to users
    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
