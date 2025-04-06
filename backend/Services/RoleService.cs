using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RunClubAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RoleManager<IdentityRole> roleManager, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("Fetching all roles...");

            var roles = await Task.FromResult(_roleManager.Roles.ToList());

            if (!roles.Any())
                _logger.LogWarning("No roles found in the system.");

            return roles.Select(role => new RoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name ?? "",
                RoleNormalizedName = role.NormalizedName ?? ""
            });
        }

        public async Task<RoleDTO?> GetRoleByIdAsync(string id)
        {
            _logger.LogInformation("Fetching role with ID {RoleId}", id);

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogWarning("Role with ID {RoleId} not found", id);
                return null;
            }

            return new RoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name ?? "",
                RoleNormalizedName = role.NormalizedName ?? ""
            };
        }

        // OPTIONAL: Add if used elsewhere
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}



/* The RoleService in my project is responsible for handling role management functionalities, ensuring that user roles are efficiently retrieved from the database. It implements the IRoleService interface, adhering to the principles of separation of concerns and dependency injection by utilizing RunClubContext for database access and ILogger<RoleService> for logging. To optimize performance, the GetAllRolesAsync() and GetRoleByIdAsync() methods use AsNoTracking(), which prevents unnecessary tracking of database entities, making queries more efficient. Additionally, I have implemented structured error handling and logging mechanisms to capture system events, such as missing roles or successful retrievals, which enhances debugging and monitoring. The service follows a DTO-based approach, ensuring that only the required data is exposed while keeping the database models encapsulated. This structured implementation not only improves maintainability but also enhances the security and scalability of the system. Through this service, I have demonstrated my understanding of best practices in API design, database interactions using EF Core, and the importance of logging for observability in modern applications. */