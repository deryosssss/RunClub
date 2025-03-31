using RunClubAPI.Interfaces;   // Provides the IUserRepository interface for enforcing repository contract.
using RunClubAPI.Models;       // Accesses the User model representing application users.
using Microsoft.EntityFrameworkCore; // Enables Entity Framework Core operations for database interactions.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Repositories // Defines the namespace for repository classes managing data persistence.
{
    /// <summary>
    /// Repository responsible for handling CRUD operations related to users.
    /// Implements IUserRepository to enforce a structured approach to data access.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly RunClubContext _context; // Database context for interacting with the Users table.

        /// <summary>
        /// Initializes the UserRepository with a database context.
        /// </summary>
        /// <param name="context">Instance of RunClubContext injected via dependency injection.</param>
        public UserRepository(RunClubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all users from the database.
        /// </summary>
        /// <returns>A collection of User objects.</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
            // Uses ToListAsync() to perform a non-blocking query, ensuring scalability.
        }

        /// <summary>
        /// Asynchronously retrieves a user by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
            // Uses FindAsync() for efficient primary key lookup, reducing database load.
        }

        /// <summary>
        /// Asynchronously adds a new user to the database.
        /// </summary>
        /// <param name="user">The User object to be added.</param>
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user); // Adds the user entity to the DbContext.
            await _context.SaveChangesAsync(); // Persists the change asynchronously.
        }

        /// <summary>
        /// Asynchronously updates an existing user.
        /// </summary>
        /// <param name="user">The updated User object.</param>
        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified; 
            // Marks the user entity as modified, ensuring only changed fields are updated.
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            // Checks if the user exists before attempting deletion.
            if (user != null)
            {
                _context.Users.Remove(user); // Marks the user entity for deletion.
                await _context.SaveChangesAsync(); // Commits the deletion to the database.
            }
        }
    }
}

/* 

The UserRepository class implements the repository pattern to handle database operations related to users. It acts as an abstraction layer between the data access logic and the business logic, making the system more modular, scalable, and maintainable.

Key Features and Benefits
Encapsulation and Separation of Concerns

The repository pattern ensures that data access logic is separated from business logic, adhering to the Single Responsibility Principle (SRP).
The application remains loosely coupled, making modifications easier.
Dependency Injection

The RunClubContext is injected via the constructor, promoting testability and flexibility.
This allows easy swapping of database contexts (e.g., using an in-memory database for testing).
Asynchronous Database Operations

The repository methods are asynchronous (async/await), ensuring non-blocking execution.
This improves application performance, especially when handling large datasets or multiple requests.
CRUD Operations

GetAllAsync(): Fetches all users asynchronously using ToListAsync(), reducing application blocking.
GetByIdAsync(int id): Uses FindAsync() for efficient retrieval using the primary key.
AddAsync(User user): Adds a new user and persists changes.
UpdateAsync(User user): Updates a user by marking it as modified.
DeleteAsync(int id): Checks if a user exists before removing it, preventing errors.
Why Use This Approach?
Scalability: The use of asynchronous methods ensures that the API can handle multiple requests efficiently.
Maintainability: The structured approach allows future developers to modify the logic without affecting business rules.
Security: Prevents direct exposure of the database context, reducing risks like SQL injection.
Testability: The repository can be mocked during unit testing, enabling isolated testing of business logic.
Final Thoughts
This UserRepository is a crucial component of the data access layer in the RunClubAPI. By implementing the repository pattern, it provides a structured, efficient, and scalable way to interact with the Users table while maintaining separation of concerns. It ensures smooth CRUD operations, improves performance through asynchronous execution, and makes the system more testable and maintainable.
*/