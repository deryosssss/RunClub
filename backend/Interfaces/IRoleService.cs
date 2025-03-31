using RunClubAPI.DTOs;  // Importing DTOs to standardize role data transfer.
using RunClubAPI.Models;  // Importing Role model to work with database entities.

namespace RunClubAPI.Interfaces
{
    // Service interface defining business logic related to roles.
    public interface IRoleService
    {
        // Fetches all roles in the system as DTOs.
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();

        // Retrieves a specific role based on its unique identifier (string ID).
        Task<RoleDTO> GetRoleByIdAsync(string id);
    }
}
/* The IRoleService interface is part of the service layer and defines business logic for role-related operations. It acts as a bridge between the RolesController and the IRoleRepository.

Why Use a Service Layer?

It decouples business logic from data access.
Allows easy modification of business rules without affecting database operations.
Enhances testability, as the service logic can be tested independently of the database.
Method Breakdown:

GetAllRolesAsync(): Returns a list of all available roles (e.g., Admin, Coach, Runner).
GetRoleByIdAsync(string id): Retrieves a role based on its string-based ID (since IdentityRole typically uses a GUID or string ID).
Why Return DTOs Instead of Models?

Encapsulation: DTOs (Data Transfer Objects) ensure only necessary role details are exposed.
Security: Prevents unwanted database fields from being leaked.
By implementing IRoleService, we ensure clean separation of concerns, better maintainability, and improved performance when handling role-related operations.*/