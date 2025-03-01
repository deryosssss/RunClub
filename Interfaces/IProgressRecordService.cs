using RunClubAPI.DTOs;  // Importing DTOs for data transfer between layers.
using RunClubAPI.Models;  // Importing the ProgressRecord model.

namespace RunClubAPI.Interfaces
{
    // Service interface for managing progress records.
    public interface IProgressRecordService
    {
        // Fetches all progress records asynchronously.
        Task<IEnumerable<ProgressRecordDTO>> GetAllProgressRecordsAsync();

        // Retrieves a specific progress record by ID.
        Task<ProgressRecordDTO> GetProgressRecordByIdAsync(int id);

        // Adds a new progress record to the system.
        Task<ProgressRecordDTO> AddProgressRecordAsync(ProgressRecordDTO progressRecordDto);

        // Deletes a progress record by ID.
        Task<bool> DeleteProgressRecordAsync(int id);

        // Updates an existing progress record.
        Task<bool> UpdateProgressRecordAsync(int id, ProgressRecordDTO progressRecordDto);
    }
}

/* The IProgressRecordService interface defines the business logic for handling progress records. This follows the Service Layer Pattern, which keeps the controller logic clean and ensures reusability.

Separation of Concerns: The service layer ensures that business rules and validation are separate from the repository and controllers.
Asynchronous Programming (async): Every method is asynchronous, improving system responsiveness and handling large datasets efficiently.
DTO Usage (ProgressRecordDTO): Instead of exposing the database entity directly, we use DTOs to control the data flow and structure.
CRUD Operations:
GetAllProgressRecordsAsync(): Retrieves all records for tracking user performance.
GetProgressRecordByIdAsync(int id): Fetches a specific progress record.
AddProgressRecordAsync(ProgressRecordDTO progressRecordDto): Adds a new progress entry.
DeleteProgressRecordAsync(int id): Removes a record from the system.
UpdateProgressRecordAsync(int id, ProgressRecordDTO progressRecordDto): Modifies an existing record.
This service will be implemented in a class (ProgressRecordService), which interacts with the repository (IProgressRecordRepository). The separation of repository, service, and controller layers makes the system modular, testable, and scalable. */
