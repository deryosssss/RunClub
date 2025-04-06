using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // GET: api/enrollments?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollments(int pageNumber = 1, int pageSize = 10)
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync(pageNumber, pageSize);
            return Ok(enrollments);
        }

        // GET: api/enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found.");

            return Ok(enrollment);
        }

        // GET: api/enrollments/event/3
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByEvent(int eventId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByEventIdAsync(eventId);
            return Ok(enrollments);
        }

        // POST: api/enrollments
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostEnrollment([FromBody] EnrollmentDTO enrollmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);

            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollmentDto.EnrollmentId }, enrollmentDto);
        }

        // DELETE: api/enrollments/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound($"Enrollment with ID {id} not found.");

            await _enrollmentService.DeleteEnrollmentAsync(id);
            return NoContent();
        }
    }
}

/* 
"This controller manages enrollments using a service layer, which ensures a clean separation of concerns."
"We use HTTP status codes effectively to provide clear responses to clients."
"Pagination helps optimize performance by fetching only a limited set of results at a time."
"Deleting an enrollment returns 204 No Content, which is the best practice for delete operations." */
