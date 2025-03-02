using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace RunClubAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RunClubContext _context;

        public RoleRepository(RunClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.Name == roleName);
        }

        public async Task AddRoleAsync(IdentityRole role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
