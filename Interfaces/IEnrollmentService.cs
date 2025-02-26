using RunClubAPI.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber = 1, int pageSize = 10);
        Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId);
        Task AddEnrollmentAsync(EnrollmentDTO enrollment);
        Task DeleteEnrollmentAsync(int id);
    }

}

/* Why These Values?
Page Number (1): By convention, pagination often starts at 1, as users generally expect the first page to be numbered as 1. Page 0 would be unusual in most pagination systems, and it's more natural to start from 1.

Page Size (10): The default of 10 is commonly chosen because it is small enough to return a reasonable amount of data without overwhelming the client, but large enough that the client doesn't need to send too many requests to get all data. It’s a typical "sweet spot" in many applications. However, you can allow users to specify a different page size if needed.
*/
