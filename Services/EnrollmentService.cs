using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RunClubAPI.Services
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

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
        {
            return await _context.Enrollments
                .Where(e => e.EventId == eventId)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    EventId = e.EventId,
                    UserId = e.UserId,
                    EnrollmentDate = e.EnrollmentDate
                })
                .ToListAsync();
        }

        public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id) // Ensure nullable return type
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


        public async Task AddEnrollmentAsync(EnrollmentDTO enrollmentDto)
        {
            _logger.LogInformation("Adding a new enrollment...");

            try
            {
                // First, check if the User and Event exist
                var user = await _context.Users.FindAsync(enrollmentDto.UserId);
                var eventEntity = await _context.Events.FindAsync(enrollmentDto.EventId);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {enrollmentDto.UserId} not found.");
                    throw new ApplicationException("User not found.");
                }

                if (eventEntity == null)
                {
                    _logger.LogWarning($"Event with ID {enrollmentDto.EventId} not found.");
                    throw new ApplicationException("Event not found.");
                }

                // Create a new Enrollment object
                var enrollment = new Enrollment
                {
                    UserId = enrollmentDto.UserId,
                    EventId = enrollmentDto.EventId,
                    User = user,       // Set the User navigation property
                    Event = eventEntity // Set the Event navigation property
                };

                // Add the enrollment to the context
                _context.Enrollments.Add(enrollment);

                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding enrollment: {ex.Message}");
                throw new ApplicationException("An error occurred while adding enrollment.");
            }
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            _logger.LogInformation($"Deleting enrollment with ID {id}");

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                _logger.LogWarning($"Enrollment with ID {id} not found.");
                return;
            }

            _context.Enrollments.Remove(enrollment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting enrollment: {ex.Message}");
                throw new ApplicationException("An error occurred while deleting enrollment.");
            }
        }
    }
}
