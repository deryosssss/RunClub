using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunClubAPI.DTOs;

namespace RunClubAPI.Controllers
{
    // This controller manages user roles in the system.
    // Access is restricted to users with the "Admin" role.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only Admins can manage roles
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager; // Handles role-related operations
        private readonly UserManager<IdentityUser> _userManager; // Manages user accounts and roles

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/Roles
        // Retrieves a list of all roles in the system.
        // Useful for admins to view available roles.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            // Fetch all roles from the database asynchronously.
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        // GET: api/Roles/{id}
        // Retrieves a specific role by its ID.
        // Allows admins to fetch role details, including normalized role names.
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(string id)
        {
            // Search for the role by its unique ID.
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(new { message = "Role not found." });
            }

            // Return the role details as a RoleDTO.
            return Ok(new RoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name,
                RoleNormalizedName = role.NormalizedName
            });
        }

        // POST: api/Roles
        // Creates a new role, ensuring that duplicate roles are not created.
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            // Ensure the role name is not already in use.
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest(new { message = "Role already exists." });
            }

            // Create a new role instance and add it to the database.
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            // Check if role creation was successful.
            if (result.Succeeded)
            {
                return Ok(new { message = "Role created successfully." });
            }

            // If errors occur, return them.
            return BadRequest(result.Errors);
        }

        // POST: api/Roles/assign-role-to-user
        // Assigns a role to a user. Prevents self-assignment of the "Admin" role.
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            // Find the user by ID.
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Ensure the role exists before assignment.
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return NotFound(new { message = "Role not found." });
            }

            // Prevent users from assigning themselves the "Admin" role.
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.Id == user.Id && model.RoleName == "Admin")
            {
                return BadRequest(new { message = "You cannot assign yourself the Admin role." });
            }

            // Assign the role to the user.
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully." });
            }

            // Return any errors encountered during assignment.
            return BadRequest(result.Errors);
        }

        // DELETE: api/Roles/{id}
        // Deletes a role by its ID, preventing deletion of the "Admin" role.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            // Find the role in the database.
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { message = "Role not found." });
            }

            // Prevent deletion of the critical "Admin" role.
            if (role.Name == "Admin")
            {
                return BadRequest(new { message = "The Admin role cannot be deleted." });
            }

            // Delete the role and check if it was successful.
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }

            // If deletion failed, return the errors.
            return BadRequest(result.Errors);
        }
    }

    // Model class used to assign a role to a user.
    public class AssignRoleModel
    {
        public string UserId { get; set; } // ID of the user receiving the role
        public string RoleName { get; set; } // Name of the role being assigned
    }
}

/* "The RolesController in my ASP.NET Core Web API is responsible for managing user roles, ensuring that only authorized administrators can create, retrieve, assign, and delete roles. This controller enforces security using [Authorize(Roles = "Admin")], restricting access to users with the 'Admin' role. It leverages dependency injection with RoleManager<IdentityRole> and UserManager<IdentityUser> to handle role and user management efficiently. Key functionalities include retrieving all roles, getting a specific role by ID, preventing duplicate role creation, and ensuring users cannot assign themselves the 'Admin' role for security purposes. Additionally, the controller prevents the deletion of the 'Admin' role to maintain system integrity. Proper validation, error handling, and DTOs are used to ensure clean API responses and prevent unauthorized operations, following best practices in secure role-based access control." */