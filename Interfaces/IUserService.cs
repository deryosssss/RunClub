using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.DTOs;

namespace RunClubAPI.Interfaces
{
    // Interface defining business logic for user management.
    public interface IUserService
    {
        // Retrieves all users with pagination support.
        Task<IEnumerable<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize);

        // Retrieves a single user by their unique ID.
        Task<UserDTO?> GetUserByIdAsync(int id);

        // Creates a new user while ensuring the role is included.
        Task<UserDTO?> CreateUserAsync(UserDTO userDto);

        // Updates an existing user’s details.
        Task<bool> UpdateUserAsync(int id, UserDTO user);

        // Deletes a user from the system.
        Task<bool> DeleteUserAsync(int id);

        // Retrieves users based on their assigned role.
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(string roleId);
    }
}

/* The IUserService interface represents the business logic layer for managing users in the system. It acts as an intermediary between the data layer (IUserRepository) and the controller layer, ensuring data consistency and validation before interacting with the database.

Why Use a Service Layer?

Separates business logic from controllers: Improves code maintainability.
Enhances security: Ensures user role validation and input sanitization before calling the repository.
Facilitates unit testing: Can be tested independently of database operations.
Method Breakdown:

GetAllUsersAsync(int pageNumber, int pageSize): Implements pagination, preventing excessive data load.
GetUserByIdAsync(int id): Fetches user details by their unique ID.
CreateUserAsync(UserDTO userDto): Ensures the role is included when creating a user.
UpdateUserAsync(int id, UserDTO user): Modifies user data while maintaining integrity.
DeleteUserAsync(int id): Removes a user based on their ID.
GetUsersByRoleAsync(string roleId): Fetches users associated with a specific role.
Why Use DTOs Instead of Direct Models?

Encapsulation: Prevents exposing database structure directly to the API.
Security: Hides sensitive fields such as passwords.
Flexibility: Allows modifying the API response structure without changing the database.
By implementing IUserService, we ensure a clean architecture, where the business logic is well-organized and adaptable to future changes. */


