using RunClubAPI.Interfaces;  // Provides access to the IEnrollmentRepository interface.
using RunClubAPI.Models;      // Includes the Enrollment model, which represents the entity being managed.
using Microsoft.EntityFrameworkCore; // Provides asynchronous database operations via Entity Framework Core.
using System.Collections.Generic; // Required for working with collections like List<T>.
using System.Threading.Tasks; // Supports asynchronous programming.

namespace RunClubAPI.Repositories
{
    /// <summary>
    /// Repository class responsible for handling data operations related to enrollments.
    /// Implements the IEnrollmentRepository interface to ensure consistency and abstraction.
    /// </summary>
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly RunClubContext _context; // Database context for accessing the Enrollment table.

        /// <summary>
        /// Constructor to initialize the repository with the database context.
        /// </summary>
        /// <param name="context">Instance of RunClubContext used for database interactions.</param>
        public EnrollmentRepository(RunClubContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all enrollments asynchronously from the database.
        /// </summary>
        /// <returns>A list of all enrollments in the database.</returns>
        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync(); 
            // Uses Entity Framework Core's ToListAsync() to efficiently fetch all records.
        }
    }
}

/* The EnrollmentRepository is a repository class in the RunClubAPI that is responsible for retrieving enrollment data from the database. It follows the Repository Pattern, which is a design principle that abstracts data access logic from the business logic, making the system more modular and maintainable. This class implements the IEnrollmentRepository interface, ensuring that the contract for data access operations is well-defined and consistent across the application.

The repository interacts with the RunClubContext, which is the Entity Framework Core (EF Core) database context. The constructor injects this context as a dependency, promoting dependency injection, which is an important practice in modern software development as it enhances testability and flexibility.

The GetAllEnrollmentsAsync() method is an asynchronous operation that retrieves all enrollments from the database using ToListAsync(). The use of async and await ensures that the method does not block the main execution thread, which improves the application's performance, particularly in web applications where multiple users may be accessing the system simultaneously.

By encapsulating database access within this repository, the system gains several advantages:

Separation of concerns: Business logic does not need to worry about database interactions.
Scalability: If the database implementation changes (e.g., switching from SQL Server to PostgreSQL), changes are isolated within the repository.
Testability: The repository can be easily mocked for unit testing, making it possible to test business logic independently.
This class is a fundamental part of the data access layer in the RunClubAPI system and plays a crucial role in managing the persistence of user enrollments in running events.*/