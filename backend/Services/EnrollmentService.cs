using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Data;

namespace RunClubAPI.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly RunClubContext _context;

        public EnrollmentService(RunClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Enrollments
                .OrderBy(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            return enrollment != null ? MapToDto(enrollment) : null;
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
        {
            return await _context.Enrollments
                .Where(e => e.EventId == eventId)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByRunnerIdAsync(string runnerId)
        {
            return await _context.Enrollments
                .Where(e => e.UserId == runnerId)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<EnrollmentDTO> CreateEnrollmentAsync(EnrollmentDTO dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId)
                ?? throw new ArgumentException("User not found");

            var @event = await _context.Events.FindAsync(dto.EventId)
                ?? throw new ArgumentException("Event not found");

            var entity = new Enrollment
            {
                EnrollmentDate = dto.EnrollmentDate,
                UserId = dto.UserId,
                EventId = dto.EventId,
                User = user,
                Event = @event
            };

            _context.Enrollments.Add(entity);
            await _context.SaveChangesAsync();

            dto.EnrollmentId = entity.EnrollmentId;
            return dto;
        }

        public async Task<bool> UpdateEnrollmentAsync(int id, EnrollmentDTO dto)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return false;

            enrollment.EnrollmentDate = dto.EnrollmentDate;
            enrollment.UserId = dto.UserId;
            enrollment.EventId = dto.EventId;

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        private EnrollmentDTO MapToDto(Enrollment e) => new()
        {
            EnrollmentId = e.EnrollmentId,
            EnrollmentDate = e.EnrollmentDate,
            UserId = e.UserId,
            EventId = e.EventId
        };
    }
}


/* The EnrollmentService class implements the IEnrollmentService interface and is responsible for managing enrollment-related business logic within the RunClubAPI system. It provides CRUD (Create, Read, Update, Delete) operations, ensuring a structured and efficient way to handle enrollment data. The service follows an asynchronous programming model, leveraging Entity Framework Core (EF Core) for database interactions. It includes pagination for efficient data retrieval, preventing performance issues with large datasets. DTOs (Data Transfer Objects) are used to decouple the internal database models from external API responses, enhancing maintainability and security. Additionally, error handling is implemented to ensure the system remains robust, preventing issues such as invalid user/event references or concurrent data modifications. The service adheres to separation of concerns, improving testability and scalability by keeping the business logic distinct from the controller layer. */