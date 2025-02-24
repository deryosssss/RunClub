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

        public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync()
        {
            _logger.LogInformation("Fetching all enrollments...");

            try
            {
                var enrollments = await _context.Enrollments.ToListAsync();

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all enrollments.");
                throw; // Re-throw to allow higher-level handling
            }
        }

        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching enrollment with ID {EnrollmentId}", id);

            try
            {
                var enrollment = await _context.Enrollments.FindAsync(id);

                if (enrollment == null)
                {
                    _logger.LogWarning("Enrollment with ID {EnrollmentId} does not exist.", id);
                    return null;
                }

                return new EnrollmentDTO
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    UserId = enrollment.UserId,
                    EventId = enrollment.EventId // Fixed incorrect field assignment
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching enrollment with ID {EnrollmentId}", id);
                throw; // Re-throw for higher-level handling
            }
        }
    }
}
