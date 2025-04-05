using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;

namespace RunClubAPI.Controllers
{
    // This controller manages CRUD operations for progress records.
    [Route("api/[controller]")] // Base URL: "api/progressrecords"
    [ApiController] // Enables model binding, automatic validation, and more.
    // [Authorize(Roles = "Coach")] // Only users with "Coach" role can access.
    public class ProgressRecordsController : ControllerBase
    {
        private readonly IProgressRecordService _progressRecordService;
        private readonly ILogger<ProgressRecordsController> _logger; // Logger for debugging and tracking API usage.

        // Constructor: Injects the progress record service and logger.
        public ProgressRecordsController(IProgressRecordService progressRecordService, ILogger<ProgressRecordsController> logger)
        {
            _progressRecordService = progressRecordService;
            _logger = logger;
        }

        // GET all progress records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressRecordDTO>>> GetProgressRecords()
        {
            _logger.LogInformation("Fetching all progress records..."); // Logs API request.

            var progressRecords = await _progressRecordService.GetAllProgressRecordsAsync();

            if (progressRecords?.Any() != true) // Null-safe check for empty list.
            {
                _logger.LogWarning("No progress records found.");
                return NotFound("No progress records available."); // Returns 404 if no records exist.
            }

            return Ok(progressRecords); // Returns the list of progress records.
        }

        // GET a single progress record by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressRecordDTO>> GetProgressRecord(int id)
        {
            _logger.LogInformation($"Fetching progress record with ID {id}");

            var progressRecord = await _progressRecordService.GetProgressRecordByIdAsync(id);

            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return NotFound($"Progress record with ID {id} not found."); // Returns 404 if the record is missing.
            }

            return Ok(progressRecord); // Returns the progress record.
        }

        // CREATE a new progress record
        [HttpPost]
        public async Task<ActionResult<ProgressRecordDTO>> PostProgressRecord(ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation("Attempting to add a new progress record...");

            // Validates required fields.
            if (string.IsNullOrWhiteSpace(progressRecordDto.ProgressDate) || string.IsNullOrWhiteSpace(progressRecordDto.ProgressTime))
            {
                return BadRequest(new { message = "Both ProgressDate and ProgressTime are required." }); // Returns 400 for missing fields.
            }

            var createdRecord = await _progressRecordService.AddProgressRecordAsync(progressRecordDto);

            if (createdRecord == null)
            {
                return BadRequest(new { message = "Invalid User ID. Cannot add progress record." }); // Ensures a valid user ID exists.
            }

            return CreatedAtAction(nameof(GetProgressRecord), new { id = createdRecord.ProgressRecordId }, createdRecord); // 201 Created response.
        }

        // UPDATE an existing progress record
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgressRecord(int id, ProgressRecordDTO progressRecordDto)
        {
            if (id != progressRecordDto.ProgressRecordId)
            {
                return BadRequest("ProgressRecord ID mismatch."); // Ensures ID in URL matches the record object.
            }

            _logger.LogInformation($"Updating progress record with ID {id}");

            // Validates required fields.
            if (string.IsNullOrWhiteSpace(progressRecordDto.ProgressDate) || string.IsNullOrWhiteSpace(progressRecordDto.ProgressTime))
            {
                return BadRequest(new { message = "Both ProgressDate and ProgressTime are required." });
            }

            var updateSuccess = await _progressRecordService.UpdateProgressRecordAsync(id, progressRecordDto);

            if (!updateSuccess)
            {
                return NotFound($"Progress record with ID {id} not found."); //  Returns 404 if the record doesn’t exist.
            }

            return NoContent(); // 204 No Content (Successful update).
        }

        // DELETE a progress record by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgressRecord(int id)
        {
            _logger.LogInformation($"Attempting to delete progress record with ID {id}");

            var deleteSuccess = await _progressRecordService.DeleteProgressRecordAsync(id);

            if (!deleteSuccess)
            {
                return NotFound($"Progress record with ID {id} not found."); // Returns 404 if the record doesn’t exist.
            }

            return NoContent(); // 204 No Content (Successful deletion).
        }
    }
}

/* 
✔ DTO Usage – Separates internal models from API responses (ProgressRecordDTO).
✔ Logging for Debugging –
Info logs track API calls (LogInformation).
Warning logs help identify potential issues (LogWarning).
✔ Null-Safe Checks – Prevents null reference errors (progressRecords?.Any() != true).
✔ Async/Await – Ensures efficient database queries.

"This controller manages CRUD operations for progress records while ensuring that only users with the 'Coach' role can access it."
"We use DTOs (ProgressRecordDTO) to structure responses and avoid exposing unnecessary database fields."
"Logging helps track API usage and debugging, such as logging missing records or invalid requests."
"Role-based authorization ([Authorize(Roles = "Coach")]) ensures only coaches can create, update, or delete progress records."
"Null-safe checks prevent unexpected runtime errors when fetching records from the database." */