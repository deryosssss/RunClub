using RunClubAPI.Interfaces;  // For IEnrollmentRepository
using RunClubAPI.Models;      // For Enrollment
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly RunClubContext _context;

        public EnrollmentRepository(RunClubContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }
    }
}

