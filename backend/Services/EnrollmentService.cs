using RunClubAPI.DTOs; // Importing DTOs for transferring enrollment data
using RunClubAPI.Interfaces; // Importing interface for dependency injection
using RunClubAPI.Models; // Importing models used in database operations
using Microsoft.EntityFrameworkCore; // Importing EF Core for database operations
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Importing async task utilities

/// <summary>
/// Service responsible for handling enrollment-related operations.
/// </summary>
public class EnrollmentService : IEnrollmentService
{
    private readonly RunClubContext _context; // Dependency injection of database context

    /// <summary>
    /// Constructor initializes the database context.
    /// </summary>
    public EnrollmentService(RunClubContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a paginated list of enrollments, sorted by enrollment date.
    /// </summary>
    public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await _context.Enrollments
            .OrderBy(e => e.EnrollmentDate) // Sort enrollments by date
            .Skip((pageNumber - 1) * pageSize) // Implement pagination
            .Take(pageSize) // Limit results per page
            .Select(e => new EnrollmentDTO // Convert to DTO format
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,
                UserId = e.UserId,
                EventId = e.EventId
            })
            .ToListAsync(); // Execute the query asynchronously
    }

    /// <summary>
    /// Retrieves an enrollment by its ID.
    /// </summary>
    public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id); // Find enrollment by primary key
        if (enrollment == null) return null; // Return null if not found

        return new EnrollmentDTO // Convert to DTO format
        {
            EnrollmentId = enrollment.EnrollmentId,
            EnrollmentDate = enrollment.EnrollmentDate,
            UserId = enrollment.UserId,
            EventId = enrollment.EventId
        };
    }

    /// <summary>
    /// Retrieves all enrollments associated with a specific event.
    /// </summary>
    public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
    {
        return await _context.Enrollments
            .Where(e => e.EventId == eventId) // Filter by event ID
            .Select(e => new EnrollmentDTO // Convert to DTO format
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,
                UserId = e.UserId,
                EventId = e.EventId
            })
            .ToListAsync();
    }

    /// <summary>
    /// Creates a new enrollment entry in the database.
    /// </summary>
    public async Task<EnrollmentDTO> CreateEnrollmentAsync(EnrollmentDTO enrollmentDto)
    {
        var enrollment = new Enrollment
        {
            EnrollmentDate = enrollmentDto.EnrollmentDate,
            UserId = enrollmentDto.UserId,
            EventId = enrollmentDto.EventId,
            User = await _context.Users.FindAsync(enrollmentDto.UserId) 
                ?? throw new ArgumentException("User not found"), // Ensure user exists
            Event = await _context.Events.FindAsync(enrollmentDto.EventId) 
                ?? throw new ArgumentException("Event not found") // Ensure event exists
        };

        _context.Enrollments.Add(enrollment); // Add enrollment to the database
        await _context.SaveChangesAsync(); // Save changes asynchronously

        return new EnrollmentDTO // Return DTO representation of the new enrollment
        {
            EnrollmentId = enrollment.EnrollmentId,
            EnrollmentDate = enrollment.EnrollmentDate,
            UserId = enrollment.UserId,
            EventId = enrollment.EventId
        };
    }

    /// <summary>
    /// Updates an existing enrollment record.
    /// </summary>
    public async Task<bool> UpdateEnrollmentAsync(int id, EnrollmentDTO enrollmentDto)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return false; // Return false if enrollment not found

        enrollment.EnrollmentDate = enrollmentDto.EnrollmentDate;
        enrollment.UserId = enrollmentDto.UserId;
        enrollment.EventId = enrollmentDto.EventId;

        _context.Entry(enrollment).State = EntityState.Modified; // Mark entity as modified

        try
        {
            await _context.SaveChangesAsync(); // Save changes asynchronously
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false; // Handle concurrency issues
        }
    }

    /// <summary>
    /// Deletes an enrollment record from the database.
    /// </summary>
    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return false; // Return false if not found

        _context.Enrollments.Remove(enrollment); // Remove from database
        await _context.SaveChangesAsync(); // Save changes asynchronously

        return true;
    }
}

/* The EnrollmentService class implements the IEnrollmentService interface and is responsible for managing enrollment-related business logic within the RunClubAPI system. It provides CRUD (Create, Read, Update, Delete) operations, ensuring a structured and efficient way to handle enrollment data. The service follows an asynchronous programming model, leveraging Entity Framework Core (EF Core) for database interactions. It includes pagination for efficient data retrieval, preventing performance issues with large datasets. DTOs (Data Transfer Objects) are used to decouple the internal database models from external API responses, enhancing maintainability and security. Additionally, error handling is implemented to ensure the system remains robust, preventing issues such as invalid user/event references or concurrent data modifications. The service adheres to separation of concerns, improving testability and scalability by keeping the business logic distinct from the controller layer. */