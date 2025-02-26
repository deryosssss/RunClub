using RunClubAPI.Models;  // For the Enrollment model.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
    }
}
