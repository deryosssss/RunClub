using RunClubAPI.Interfaces;   // Provides access to the IProgressRecordRepository interface.
using RunClubAPI.Models;       // Includes the ProgressRecord model, representing the entity being managed.
using Microsoft.EntityFrameworkCore; // Enables asynchronous database operations using Entity Framework Core.
using RunClubAPI.DTOs;         // (Currently unused in this class, may be required for future enhancements.)

namespace RunClubAPI.Repositories
{
    // Repository class responsible for handling database operations related to progress records.
    // Implements the IProgressRecordRepository interface for consistency and abstraction.
    public class ProgressRecordRepository : IProgressRecordRepository
    {
        private readonly RunClubContext _context; // Database context to interact with the ProgressRecord table.

        // Initializes a new instance of the <see cref="ProgressRecordRepository"/> class.
        // Uses dependency injection to provide access to the database context.
        public ProgressRecordRepository(RunClubContext context)
        {
            _context = context;
        }

        // Retrieves all progress records asynchronously from the database.
        // </summary>
        // <returns>A list of all progress records stored in the database.</returns>
        public async Task<List<ProgressRecord>> GetAllProgressRecordsAsync()
        {
            return await _context.ProgressRecords.ToListAsync();
            // Uses Entity Framework Core's ToListAsync() to efficiently fetch all records asynchronously.
        }
    }
}

/* The ProgressRecordRepository class is part of the RunClubAPI and is responsible for retrieving progress records from the database. It follows the Repository Pattern, which abstracts the data access layer from the business logic. This abstraction provides modularity, making the system more maintainable and scalable.

The class implements the IProgressRecordRepository interface, ensuring that the repository follows a contract-based approach to data retrieval. This design allows for easier modifications and testing since the interface defines a clear structure for data operations.

A key aspect of this class is dependency injection, where the RunClubContext is passed into the constructor. This design pattern:

Promotes loose coupling, making it easier to switch database contexts if needed.
Enhances testability, as a mock database context can be injected during unit tests.
Improves scalability, allowing for better handling of database queries.
The GetAllProgressRecordsAsync() method is an asynchronous method that retrieves all records from the ProgressRecords table. The use of await _context.ProgressRecords.ToListAsync() ensures non-blocking execution, which is crucial for web applications where multiple users might be making requests simultaneously.

By encapsulating database access in this repository, the system gains:

Better separation of concerns, keeping data retrieval separate from business logic.
Database flexibility, as the repository shields the rest of the application from direct database dependencies.
Performance improvements, leveraging asynchronous execution to handle database operations efficiently.
This repository is an essential part of the data access layer in the RunClubAPI, enabling efficient storage and retrieval of user progress records related to running activities.
*/