using RunClubAPI.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber, int pageSize);
    Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id);
    Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId);
    Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByRunnerIdAsync(string userId, bool? isCompleted = null);
    Task<EnrollmentDTO> CreateEnrollmentAsync(EnrollmentDTO dto);
    Task<bool> DeleteEnrollmentAsync(int enrollmentId);
    Task<bool> CheckIfAlreadyEnrolledAsync(string userId, int eventId);
    Task<bool> UpdateCompletionStatusAsync(int enrollmentId, string userId, bool isCompleted);
}

}
/* Pagination Defaults Explained:
- **Page Number (1)**: Standard convention for pagination starts at **1** (not 0) since users expect the first page to be numbered 1.
- **Page Size (10)**: Defaulting to **10** prevents overwhelming the client while reducing the number of requests needed to fetch all data.
*/

/*
The IEnrollmentService interface defines a structured way to manage enrollments within the system. It follows the Service Layer Pattern, keeping business logic separate from controllers.

Asynchronous Methods: Every method is async, ensuring non-blocking operations and improving performance.
Pagination in GetAllEnrollmentsAsync(): By using pageNumber and pageSize, we efficiently handle large datasets without overloading the system.
CRUD Operations: The interface provides methods to:
Retrieve all enrollments (GetAllEnrollmentsAsync).
Fetch a specific enrollment (GetEnrollmentByIdAsync).
Find enrollments for a given event (GetEnrollmentsByEventIdAsync).
Add a new enrollment (CreateEnrollmentAsync).
Update an existing enrollment (UpdateEnrollmentAsync).
Remove an enrollment (DeleteEnrollmentAsync).
By using dependency injection, controllers can interact with this interface without tightly coupling to a specific implementation. This improves scalability, testability, and maintainability of the code */