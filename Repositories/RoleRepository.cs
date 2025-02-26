using RunClubAPI.Models; // For the Role model.
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Interfaces; // Add this to reference the IRoleRepository interface.

namespace RunClubAPI.Repositories  // Ensure this is the correct namespace.
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RunClubContext _context;

        public RoleRepository(RunClubContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
    }
}
