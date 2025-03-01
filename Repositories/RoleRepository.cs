using RunClubAPI.Models;          // Provides access to the Role model, representing user roles.
using Microsoft.EntityFrameworkCore; // Enables asynchronous database operations using Entity Framework Core.
using RunClubAPI.Interfaces;      // References the IRoleRepository interface for dependency injection and abstraction.

namespace RunClubAPI.Repositories  // Defines the namespace for repository classes handling data access.
{
    /// <summary>
    /// Repository class responsible for managing role-related database operations.
    /// Implements the IRoleRepository interface to enforce consistency and separation of concerns.
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly RunClubContext _context; // Database context instance for interacting with the Roles table.

        /// <summary>
        /// Constructor that initializes the repository with a database context.
        /// Uses dependency injection to provide an instance of RunClubContext.
        /// </summary>
        /// <param name="context">The database context used for role data operations.</param>
        public RoleRepository(RunClubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all roles from the database.
        /// </summary>
        /// <returns>A list of Role objects representing all available roles.</returns>
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
            // Uses Entity Framework Core's ToListAsync() to efficiently fetch all roles in an asynchronous manner.
        }

        /// <summary>
        /// Asynchronously retrieves a specific role by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the role.</param>
        /// <returns>A Role object if found; otherwise, null.</returns>
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
            // Uses FindAsync() to locate a role by its primary key efficiently.
        }
    }
}

/* The RoleRepository class is an implementation of the repository pattern, responsible for handling data access related to user roles in the RunClubAPI. By encapsulating database operations, this class improves modularity, making it easier to manage role-related data while adhering to the Single Responsibility Principle (SRP).

This repository implements the IRoleRepository interface, enforcing a contract-based approach that guarantees a consistent structure for role-related database operations. This approach provides benefits such as:

Loose coupling, allowing for easy modifications without affecting other parts of the system.
Improved testability, as the repository can be easily mocked for unit testing.
Scalability, as it supports future expansions without directly modifying the database logic.
Key Features of the RoleRepository
Dependency Injection

The constructor accepts RunClubContext as a parameter, allowing the repository to interact with the database efficiently.
This pattern promotes maintainability and reduces direct dependencies on database logic.
Asynchronous Data Access

GetAllRolesAsync() uses ToListAsync(), ensuring non-blocking execution, which is crucial for high-performance web applications.
GetRoleByIdAsync(int id) leverages FindAsync(id), which optimizes queries by using the primary key index, reducing database lookup time.
Separation of Concerns

The repository abstracts database operations from business logic, ensuring a clean architecture.
This makes the application easier to maintain and less prone to errors when modifying the database layer.
In summary, this repository class is an essential part of the data access layer of the RunClubAPI. It ensures efficient retrieval and management of user roles, making it an integral component of authentication and authorization processes in the system.
*/