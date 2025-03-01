using RunClubAPI.Models;  // Importing the Role model for database operations.
using Microsoft.EntityFrameworkCore;  // Entity Framework Core for database interaction.
using RunClubAPI.DTOs;  // Importing DTOs to standardize data transfer.

namespace RunClubAPI.Interfaces
{
    // Interface defining role-related database operations.
    public interface IRoleRepository
    {
        // Retrieves all roles from the database asynchronously.
        Task<List<Role>> GetAllRolesAsync();

        // Fetches a specific role by its ID.
        Task<Role> GetRoleByIdAsync(int id);
    }
}

/* The IRoleRepository interface is responsible for data access related to roles in the system. This follows the Repository Pattern, which abstracts database operations and ensures cleaner code.

Separation of Concerns:
The repository only deals with data persistence and retrieval.
The service layer (IRoleService) will handle business logic.
The controller (RolesController) will process HTTP requests.
Asynchronous Methods (async): Improves scalability by preventing blocking operations.
Method Breakdown:
GetAllRolesAsync(): Fetches a list of all available roles (e.g., Admin, Runner, Coach).
GetRoleByIdAsync(int id): Retrieves a specific role based on its unique identifier.
By using an interface, we decouple the implementation from the business logic, making the system more maintainable, testable, and scalable. The actual implementation (RoleRepository) will interact with RunClubContext using Entity Framework Core to perform queries efficiently. */
