using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}


/* The IUserRepository interface is part of the repository pattern, which abstracts database operations and provides a structured way to manage user-related data.

Why Use a Repository Layer?

Separation of concerns: It isolates data access logic from business logic.
Easier unit testing: Can be mocked independently of the database.
Maintainability: Any change in data storage (e.g., switching from SQL Server to MongoDB) only affects this layer.
Method Breakdown:

GetAllAsync(): Retrieves all users as a list asynchronously.
GetByIdAsync(int id): Finds a user by their unique ID.
AddAsync(User user): Saves a new user in the database.
UpdateAsync(User user): Modifies an existing user's data.
DeleteAsync(int id): Removes a user based on their ID.
Why Use Task<> and async?

Improves scalability: Non-blocking I/O allows efficient handling of multiple database requests.
Prevents UI freezing: Keeps the application responsive.
By implementing IUserRepository, we decouple the data access logic from the rest of the system, ensuring flexibility and efficiency in managing user-related operations */