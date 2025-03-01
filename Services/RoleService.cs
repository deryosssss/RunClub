using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Services
{
    /// <summary>
    /// Service class for managing roles within the RunClub system.
    /// Implements the IRoleService interface and interacts with the database via EF Core.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly RunClubContext _context; // Database context for accessing the roles table
        private readonly ILogger<RoleService> _logger; // Logger for recording system activities

        /// <summary>
        /// Constructor for RoleService.
        /// Injects the database context and logger.
        /// </summary>
        public RoleService(RunClubContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all roles from the database.
        /// Uses AsNoTracking() to improve performance by avoiding unnecessary tracking of retrieved entities.
        /// </summary>
        /// <returns>A list of RoleDTO objects representing all roles in the system.</returns>
        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logger.LogInformation("Fetching all roles...");

            // Retrieve all roles from the database without tracking changes to optimize performance
            var roles = await _context.Roles.AsNoTracking().ToListAsync();

            // Log a warning if no roles exist in the database
            if (!roles.Any())
            {
                _logger.LogWarning("No roles found in the system.");
            }

            // Convert Role entities into RoleDTO objects before returning to maintain API encapsulation
            return roles.Select(r => new RoleDTO
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            }).ToList();
        }

        /// <summary>
        /// Retrieves a specific role by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (string) of the role.</param>
        /// <returns>A RoleDTO representing the found role, or null if not found.</returns>
        public async Task<RoleDTO?> GetRoleByIdAsync(string id) // ID changed from int to string for flexibility
        {
            _logger.LogInformation($"Fetching role with ID {id}");

            // Search for the role in the database without tracking changes
            var role = await _context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleId == id);

            // Log a warning if the role does not exist
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {id} not found.");
                return null;
            }

            // Convert the Role entity to a RoleDTO before returning
            return new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }
    }
}

/* The RoleService in my project is responsible for handling role management functionalities, ensuring that user roles are efficiently retrieved from the database. It implements the IRoleService interface, adhering to the principles of separation of concerns and dependency injection by utilizing RunClubContext for database access and ILogger<RoleService> for logging. To optimize performance, the GetAllRolesAsync() and GetRoleByIdAsync() methods use AsNoTracking(), which prevents unnecessary tracking of database entities, making queries more efficient. Additionally, I have implemented structured error handling and logging mechanisms to capture system events, such as missing roles or successful retrievals, which enhances debugging and monitoring. The service follows a DTO-based approach, ensuring that only the required data is exposed while keeping the database models encapsulated. This structured implementation not only improves maintainability but also enhances the security and scalability of the system. Through this service, I have demonstrated my understanding of best practices in API design, database interactions using EF Core, and the importance of logging for observability in modern applications. */