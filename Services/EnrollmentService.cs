using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EnrollmentService : IEnrollmentService
{
    private readonly RunClubContext _context;

    public EnrollmentService(RunClubContext context)
    {
        _context = context;
    }

    // ✅ GET All Enrollments (Paginated)
    public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollmentsAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await _context.Enrollments
            .OrderBy(e => e.EnrollmentDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,  // ✅ Proper DateOnly handling
                UserId = e.UserId,
                EventId = e.EventId
            })
            .ToListAsync();
    }

    // ✅ GET Enrollment By ID
    public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return null;

        return new EnrollmentDTO
        {
            EnrollmentId = enrollment.EnrollmentId,
            EnrollmentDate = enrollment.EnrollmentDate,  // ✅ Correctly returning DateOnly
            UserId = enrollment.UserId,
            EventId = enrollment.EventId
        };
    }

    // ✅ GET Enrollments By Event
    public async Task<IEnumerable<EnrollmentDTO>> GetEnrollmentsByEventIdAsync(int eventId)
    {
        return await _context.Enrollments
            .Where(e => e.EventId == eventId)
            .Select(e => new EnrollmentDTO
            {
                EnrollmentId = e.EnrollmentId,
                EnrollmentDate = e.EnrollmentDate,  // ✅ Ensure correct handling
                UserId = e.UserId,
                EventId = e.EventId
            })
            .ToListAsync();
    }

    // ✅ CREATE Enrollment
    public async Task<EnrollmentDTO> CreateEnrollmentAsync(EnrollmentDTO enrollmentDto)
    {
        var enrollment = new Enrollment
        {
            EnrollmentDate = enrollmentDto.EnrollmentDate,
            UserId = enrollmentDto.UserId,
            EventId = enrollmentDto.EventId,
            User = await _context.Users.FindAsync(enrollmentDto.UserId) 
                ?? throw new ArgumentException("User not found"),
            Event = await _context.Events.FindAsync(enrollmentDto.EventId) 
                ?? throw new ArgumentException("Event not found")
        };


        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return new EnrollmentDTO
        {
            EnrollmentId = enrollment.EnrollmentId,
            EnrollmentDate = enrollment.EnrollmentDate,
            UserId = enrollment.UserId,
            EventId = enrollment.EventId
        };
    }

    // ✅ UPDATE Enrollment
    public async Task<bool> UpdateEnrollmentAsync(int id, EnrollmentDTO enrollmentDto)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return false;

        enrollment.EnrollmentDate = enrollmentDto.EnrollmentDate;  // ✅ Ensure DateOnly is updated
        enrollment.UserId = enrollmentDto.UserId;
        enrollment.EventId = enrollmentDto.EventId;

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

    // ✅ DELETE Enrollment
    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return false;

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return true;
    }
}
