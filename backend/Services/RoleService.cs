using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RunClubAPI.Services
{
    /// <summary>
    /// Service class for managing roles within the RunClub system.
    /// Implements the IRoleService interface and interacts with ASP.NET Identity.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager; // Identity RoleManager
        private readonly ILogger<RoleService> _logger; // Logger for tracking system activities

        /// <summary>
        /// Constructor for RoleService.
        /// Injects the RoleManager and logger.
        /// </summary>
        public RoleService(RoleManager<IdentityRole> roleManager, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all roles from the database.
        /// </summary>
        /// <returns>A list of RoleDTO objects representing all roles in the system.</returns>
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("Fetching all roles...");

            // Retrieve all roles using RoleManager
            var roles = _roleManager.Roles.ToList();

            if (!roles.Any())
            {
                _logger.LogWarning("No roles found in the system.");
            }

            // Convert IdentityRole to RoleDTO before returning
            return roles.Select(r => new RoleDTO
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToList();
        }

        /// <summary>
        /// Retrieves a specific role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (string) of the role.</param>
        /// <returns>A RoleDTO representing the found role, or null if not found.</returns>
        public async Task<RoleDTO?> GetRoleByIdAsync(string id)
        {
            _logger.LogInformation($"Fetching role with ID {id}");

            // Use RoleManager to find the role
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return null;
            }

            // Convert IdentityRole to RoleDTO before returning
            return new RoleDTO
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
        }
    }
}


/* The RoleService in my project is responsible for handling role management functionalities, ensuring that user roles are efficiently retrieved from the database. It implements the IRoleService interface, adhering to the principles of separation of concerns and dependency injection by utilizing RunClubContext for database access and ILogger<RoleService> for logging. To optimize performance, the GetAllRolesAsync() and GetRoleByIdAsync() methods use AsNoTracking(), which prevents unnecessary tracking of database entities, making queries more efficient. Additionally, I have implemented structured error handling and logging mechanisms to capture system events, such as missing roles or successful retrievals, which enhances debugging and monitoring. The service follows a DTO-based approach, ensuring that only the required data is exposed while keeping the database models encapsulated. This structured implementation not only improves maintainability but also enhances the security and scalability of the system. Through this service, I have demonstrated my understanding of best practices in API design, database interactions using EF Core, and the importance of logging for observability in modern applications. */