using Microsoft.AspNetCore.Mvc;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    // This controller handles CRUD operations for enrollments in events.
    [Route("api/[controller]")] // Base route: "api/enrollments"
    [ApiController] // Enables automatic model validation & request binding.
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService; // Service for enrollment logic.

        // Constructor: Injects Enrollment Service
        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // GET all enrollments (with pagination support)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollments(int pageNumber = 1, int pageSize = 10)
        {
            // Fetch paginated enrollments
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync(pageNumber, pageSize);
            return Ok(enrollments); // Returns list of enrollments.
        }

        // GET a single enrollment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound(); // Returns 404 if the enrollment doesn't exist.

            return Ok(enrollment); // Returns the found enrollment.
        }

        // GET enrollments for a specific event
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetEnrollmentsByEvent(int eventId)
        {
            // Fetch all enrollments for a given event ID
            var enrollments = await _enrollmentService.GetEnrollmentsByEventIdAsync(eventId);
            return Ok(enrollments);
        }

        // POST create a new enrollment
        [HttpPost]
        public async Task<IActionResult> PostEnrollment(EnrollmentDTO enrollmentDto)
        {
            // Call the service to create the enrollment
            await _enrollmentService.CreateEnrollmentAsync(enrollmentDto);

            // Returns HTTP 201 Created with the newly created resource
            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollmentDto.EnrollmentId }, enrollmentDto);
        }

        // DELETE an enrollment by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
                return NotFound(); // Returns 404 if the enrollment is not found.

            // 🔹 Call the service to delete the enrollment
            await _enrollmentService.DeleteEnrollmentAsync(id);
            return NoContent(); // Returns 204 No Content after successful deletion.
        }
    }
}
/* How to Explain This in Your Viva
"This controller manages enrollments using a service layer, which ensures a clean separation of concerns."
"We use HTTP status codes effectively to provide clear responses to clients."
"Pagination helps optimize performance by fetching only a limited set of results at a time."
"Deleting an enrollment returns 204 No Content, which is the best practice for delete operations." */
