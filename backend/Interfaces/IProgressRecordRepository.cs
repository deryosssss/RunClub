using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IProgressRecordRepository
    {
        Task<List<ProgressRecord>> GetAllProgressRecordsAsync();
    }
}


/* The IProgressRecordRepository interface follows the Repository Pattern, ensuring that database logic is separate from business logic.

Purpose of the Repository: This interface defines data access operations for the ProgressRecord model. By using this pattern, we ensure that controllers or services do not directly interact with the database.
Asynchronous Programming (async): The method GetAllProgressRecordsAsync() is asynchronous, ensuring non-blocking I/O operations and improving system performance.
Return Type (List<ProgressRecord>): The method returns a list of progress records, making it easier to process and display user performance data.
This repository will be implemented by a concrete class (ProgressRecordRepository), which will use Entity Framework Core to fetch progress records from the database. The repository pattern also makes the system scalable, testable, and maintainable.*/
