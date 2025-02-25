using RunClubAPI.Models;
using RunClub.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RunClub.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly RunClubContext _context;
        private readonly ILogger<EnrollmentService> _logger;

        public EnrollmentService(RunClubContext context, ILogger<EnrollmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation($"Fetching enrollments (Page {pageNumber}, Page Size {pageSize})");

            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize) // ✅ Pagination: Skip previous pages
                .Take(pageSize)  // ✅ Pagination: Limit results per page
                .ToListAsync();

            if (!enrollments.Any())
            {
                _logger.LogWarning("No enrollments found in the system.");
            }

            return enrollments.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.EnrollmentId,
                UserId = e.UserId,
                EventId = e.EventId
            }).ToList();
        }


        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching enrollment with ID {id}");

            var enrollment = await _context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null)
            {
                _logger.LogError($"Enrollment with ID {id} does not exist.");
                return null;
            }

            return new EnrollmentDTO
            {
                EnrollmentId = enrollment.EnrollmentId,
                UserId = enrollment.UserId,
                EventId = enrollment.EventId
            };
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
        {
            _logger.LogInformation($"Fetching enrollments for Event ID {eventId}");

            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.EventId == eventId) // ✅ Filtering
                .ToListAsync();

            if (!enrollments.Any())
            {
                _logger.LogWarning($"No enrollments found for Event ID {eventId}.");
            }

            return enrollments.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.EnrollmentId,
                UserId = e.UserId,
                EventId = e.EventId
            }).ToList();
        }

    }
}
