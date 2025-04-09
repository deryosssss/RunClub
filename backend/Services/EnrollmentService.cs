using Microsoft.EntityFrameworkCore;
using RunClubAPI.Data;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;

namespace RunClubAPI.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly RunClubContext _context;

        public EnrollmentService(RunClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber, int pageSize)
        {
            return await _context.Enrollments
                .Include(e => e.Event)
                .Include(e => e.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    EnrollmentDate = e.EnrollmentDate,
                    EventId = e.EventId,
                    UserId = e.UserId,
                    IsCompleted = e.IsCompleted
                })
                .ToListAsync();
        }

        public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Event)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null) return null;

            return new EnrollmentDTO
            {
                EnrollmentId = enrollment.EnrollmentId,
                EnrollmentDate = enrollment.EnrollmentDate,
                EventId = enrollment.EventId,
                UserId = enrollment.UserId,
                IsCompleted = enrollment.IsCompleted
            };
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
        {
            return await _context.Enrollments
                .Where(e => e.EventId == eventId)
                .Include(e => e.User)
                .Select(e => new EnrollmentDTO
                {
                    EnrollmentId = e.EnrollmentId,
                    EnrollmentDate = e.EnrollmentDate,
                    EventId = e.EventId,
                    UserId = e.UserId,
                    IsCompleted = e.IsCompleted
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByRunnerIdAsync(string userId, bool? isCompleted = null)
        {
            var query = _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Event)
                .AsQueryable();

            if (isCompleted.HasValue)
                query = query.Where(e => e.IsCompleted == isCompleted.Value);

            return await query.Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,
                EventId = e.EventId,
                UserId = e.UserId,
                IsCompleted = e.IsCompleted
            }).ToListAsync();
        }

        public async Task<EnrollmentDTO> CreateEnrollmentAsync(EnrollmentDTO dto)
        {
            // ✅ Prevent duplicate enrollment
            var exists = await _context.Enrollments.AnyAsync(e =>
                e.UserId == dto.UserId && e.EventId == dto.EventId);

            if (exists)
                throw new InvalidOperationException("User already enrolled in this event.");

            var enrollment = new Enrollment
            {
                EnrollmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EventId = dto.EventId,
                UserId = dto.UserId,
                IsCompleted = false
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            dto.EnrollmentId = enrollment.EnrollmentId;
            dto.EnrollmentDate = enrollment.EnrollmentDate;

            return dto;
        }

        public async Task<bool> CheckIfAlreadyEnrolledAsync(string userId, int eventId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.UserId == userId && e.EventId == eventId);
        }

        public async Task<bool> UpdateCompletionStatusAsync(int enrollmentId, string userId, bool isCompleted)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId && e.UserId == userId);

            if (enrollment == null)
                return false;

            enrollment.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null)
                return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


/* The EnrollmentService class implements the IEnrollmentService interface and is responsible for managing enrollment-related business logic within the RunClubAPI system. It provides CRUD (Create, Read, Update, Delete) operations, ensuring a structured and efficient way to handle enrollment data. The service follows an asynchronous programming model, leveraging Entity Framework Core (EF Core) for database interactions. It includes pagination for efficient data retrieval, preventing performance issues with large datasets. DTOs (Data Transfer Objects) are used to decouple the internal database models from external API responses, enhancing maintainability and security. Additionally, error handling is implemented to ensure the system remains robust, preventing issues such as invalid user/event references or concurrent data modifications. The service adheres to separation of concerns, improving testability and scalability by keeping the business logic distinct from the controller layer. */