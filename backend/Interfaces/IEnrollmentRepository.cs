using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
    }
}


/* The IEnrollmentRepository interface defines the contract for handling enrollment-related database operations in a structured and modular way. It follows the Repository Pattern, ensuring that database logic is separated from business logic.

The method GetAllEnrollmentsAsync() is asynchronous, improving performance by preventing blocking operations.
It returns a List<Enrollment>, ensuring that all enrollments in the system can be retrieved efficiently.
By using an interface, we promote flexibility and maintainability, allowing multiple database implementations (e.g., SQL, NoSQL, or in-memory databases) to be used interchangeably.
This interface is particularly useful when implementing dependency injection, allowing services to interact with enrollments without depending on a specific database technology. */