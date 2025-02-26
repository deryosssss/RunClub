using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly RunClubContext _context;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RunClubContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Fetch all roles
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("Fetching all roles...");

            var roles = await _context.Roles.AsNoTracking().ToListAsync();

            if (!roles.Any())
            {
                _logger.LogWarning("No roles found in the system.");
            }

            return roles.Select(r => new RoleDTO
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToList();
        }

        // ✅ Fetch role by ID
        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching role with ID {id}");

            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return null;
            }

            return new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }
    }
}
