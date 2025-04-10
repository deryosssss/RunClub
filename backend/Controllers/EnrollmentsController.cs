using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using RunClubAPI.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly RunClubContext _context;

        public EnrollmentsController(IEnrollmentService enrollmentService, RunClubContext context)
        {
            _enrollmentService = enrollmentService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollments(int pageNumber = 1, int pageSize = 10)
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync(pageNumber, pageSize);
            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found.");

            return Ok(enrollment);
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByEvent(int eventId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByEventIdAsync(eventId);
            return Ok(enrollments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostEnrollment([FromBody] EnrollmentDTO enrollmentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token");

                enrollmentDto.UserId = userId;

                var alreadyEnrolled = await _enrollmentService.CheckIfAlreadyEnrolledAsync(userId, enrollmentDto.EventId);
                if (alreadyEnrolled)
                    return Conflict("User is already enrolled in this event.");

                var created = await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);
                return CreatedAtAction(nameof(GetEnrollment), new { id = created.EnrollmentId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Failed to enroll: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] EnrollmentStatusUpdateDto dto)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound(new { message = "Enrollment not found" });

            enrollment.IsCompleted = dto.IsCompleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                return NotFound();

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetMyEnrollments([FromQuery] bool? isCompleted = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var enrollments = await _enrollmentService.GetEnrollmentsByRunnerIdAsync(userId, isCompleted);
            return Ok(enrollments);
        }
    }
}
/* 
"This controller manages enrollments using a service layer, which ensures a clean separation of concerns."
"We use HTTP status codes effectively to provide clear responses to clients."
"Pagination helps optimize performance by fetching only a limited set of results at a time."
"Deleting an enrollment returns 204 No Content, which is the best practice for delete operations." */
