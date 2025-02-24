using RunClub.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync();
        Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id);
        Task AddEnrollmentAsync(EnrollmentDTO enrollment);
        Task DeleteEnrollmentAsync(int id);
    }
}