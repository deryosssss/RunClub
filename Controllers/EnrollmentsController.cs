using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Models;
using RunClubAPI.DTOs;
using RunClubAPI.Services;
using RunClubAPI.Interfaces; // Add this if not already included

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly RunClubContext _context;
        private readonly IEnrollmentService _enrollmentService;  // Add this line

        public EnrollmentsController(RunClubContext context, IEnrollmentService enrollmentService)  // Inject IEnrollmentService
        {
            _context = context;
            _enrollmentService = enrollmentService;  // Assign injected service
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollments(int pageNumber = 1, int pageSize = 10)
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync(pageNumber, pageSize);
            return Ok(enrollments);
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // GET: api/Enrollments/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByEvent(int eventId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByEventIdAsync(eventId);
            return Ok(enrollments);
        }

        // PUT: api/Enrollments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return BadRequest();
            }

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Enrollments
        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentId }, enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}

