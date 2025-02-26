using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Ensure only Admins can manage roles
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityRole>> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }

            return BadRequest(result.Errors);
        }

        // POST: api/Roles/assign-role-to-user
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

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok("Role assigned successfully.");
            }

            return BadRequest(result.Errors);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
    }

    // Model to handle assigning roles to users
    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using RunClubAPI.Models;

// namespace RunClub.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
    
//     public class RolesController : ControllerBase
//     {
//         private readonly RunClubContext _context;

//         public RolesController(RunClubContext context)
//         {
//             _context = context;
//         }

//         // GET: api/Roles
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
//         {
//             return await _context.Roles.ToListAsync();
//         }

//         // GET: api/Roles/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Role>> GetRole(int id)
//         {
//             var role = await _context.Roles.FindAsync(id);

//             if (role == null)
//             {
//                 return NotFound();
//             }

//             return role;
//         }

//         // PUT: api/Roles/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutRole(int id, Role role)
//         {
//             if (id != role.RoleId)
//             {
//                 return BadRequest();
//             }

//             _context.Entry(role).State = EntityState.Modified;

//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!RoleExists(id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }

//             return NoContent();
//         }

//         // POST: api/Roles
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<Role>> PostRole(Role role)
//         {
//             _context.Roles.Add(role);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
//         }

//         // DELETE: api/Roles/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteRole(int id)
//         {
//             var role = await _context.Roles.FindAsync(id);
//             if (role == null)
//             {
//                 return NotFound();
//             }

//             _context.Roles.Remove(role);
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }

//         private bool RoleExists(int id)
//         {
//             return _context.Roles.Any(e => e.RoleId == id);
//         }
//     }
// }
