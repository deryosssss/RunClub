using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // ✅ GET all enrollments (Paginated)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollments(int pageNumber = 1, int pageSize = 10)
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync(pageNumber, pageSize);
            return Ok(enrollments);
        }

        // ✅ GET enrollment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound();

            return Ok(enrollment);
        }

        // ✅ GET enrollments for a specific event
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByEvent(int eventId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByEventIdAsync(eventId);
            return Ok(enrollments);
        }

        // ✅ POST create a new enrollment
        [HttpPost]
        public async Task<IActionResult> PostEnrollment(EnrollmentDTO enrollmentDto)
        {
            await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);
            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollmentDto.EnrollmentId }, enrollmentDto);
        }

        // ✅ DELETE enrollment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound();

            await _enrollmentService.DeleteEnrollmentAsync(id);
            return NoContent();
        }
    }
}

