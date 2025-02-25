using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            try
            {
                _logger.LogInformation($"Fetching enrollments (Page {pageNumber}, Page Size {pageSize})");

                var enrollments = await _context.Enrollments
                    .AsNoTracking()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!enrollments.Any())
                {
                    _logger.LogWarning("No enrollments found.");
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
                _logger.LogError(ex, "Error fetching enrollments.");
                throw new ApplicationException("An error occurred while retrieving enrollments.");
            }
        }

        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching enrollment with ID {id}");

                var enrollment = await _context.Enrollments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EnrollmentId == id);

                if (enrollment == null)
                {
                    _logger.LogWarning($"Enrollment with ID {id} not found.");
                    return null;
                }

                return new EnrollmentDTO
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    UserId = enrollment.UserId,
                    EventId = enrollment.EventId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching enrollment with ID {id}");
                throw new ApplicationException($"An error occurred while retrieving enrollment {id}.");
            }
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
        {
            try
            {
                _logger.LogInformation($"Fetching enrollments for Event ID {eventId}");

                bool eventExists = await _context.Events.AnyAsync(e => e.EventId == eventId);
                if (!eventExists)
                {
                    _logger.LogWarning($"Event with ID {eventId} does not exist.");
                    return Enumerable.Empty<EnrollmentDTO>(); 
                }

                var enrollments = await _context.Enrollments
                    .AsNoTracking()
                    .Where(e => e.EventId == eventId)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching enrollments for event ID {eventId}");
                throw new ApplicationException($"An error occurred while retrieving enrollments for event {eventId}.");
            }
        }
    }
}
