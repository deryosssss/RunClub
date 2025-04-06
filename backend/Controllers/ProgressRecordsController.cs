using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressRecordsController : ControllerBase
    {
        private readonly IProgressRecordService _progressRecordService;
        private readonly ILogger<ProgressRecordsController> _logger;

        public ProgressRecordsController(IProgressRecordService progressRecordService, ILogger<ProgressRecordsController> logger)
        {
            _progressRecordService = progressRecordService;
            _logger = logger;
        }

        /// <summary>Get all progress records</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProgressRecordDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProgressRecords()
        {
            _logger.LogInformation("Fetching all progress records...");

            var records = await _progressRecordService.GetAllProgressRecordsAsync();

            if (records == null || !records.Any())
            {
                _logger.LogWarning("No progress records found.");
                return NotFound("No progress records available.");
            }

            return Ok(records);
        }

        /// <summary>Get progress record by ID</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProgressRecordDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProgressRecord(int id)
        {
            _logger.LogInformation("Fetching progress record with ID {Id}", id);

            var record = await _progressRecordService.GetProgressRecordByIdAsync(id);
            if (record == null)
            {
                _logger.LogWarning("Progress record with ID {Id} not found.", id);
                return NotFound($"Progress record with ID {id} not found.");
            }

            return Ok(record);
        }

        /// <summary>Create a new progress record</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProgressRecordDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostProgressRecord([FromBody] ProgressRecordDTO dto)
        {
            _logger.LogInformation("Attempting to add a new progress record...");

            if (string.IsNullOrWhiteSpace(dto.ProgressDate) || string.IsNullOrWhiteSpace(dto.ProgressTime))
                return BadRequest(new { message = "ProgressDate and ProgressTime are required." });

            var created = await _progressRecordService.AddProgressRecordAsync(dto);

            if (created == null)
                return BadRequest(new { message = "Invalid User ID. Cannot add progress record." });

            return CreatedAtAction(nameof(GetProgressRecord), new { id = created.ProgressRecordId }, created);
        }

        /// <summary>Update a progress record</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutProgressRecord(int id, [FromBody] ProgressRecordDTO dto)
        {
            if (id != dto.ProgressRecordId)
                return BadRequest("ProgressRecord ID mismatch.");

            _logger.LogInformation("Updating progress record with ID {Id}", id);

            if (string.IsNullOrWhiteSpace(dto.ProgressDate) || string.IsNullOrWhiteSpace(dto.ProgressTime))
                return BadRequest(new { message = "ProgressDate and ProgressTime are required." });

            var updated = await _progressRecordService.UpdateProgressRecordAsync(id, dto);

            return updated
                ? NoContent()
                : NotFound($"Progress record with ID {id} not found.");
        }

        /// <summary>Delete a progress record</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProgressRecord(int id)
        {
            _logger.LogInformation("Deleting progress record with ID {Id}", id);

            var deleted = await _progressRecordService.DeleteProgressRecordAsync(id);

            return deleted
                ? NoContent()
                : NotFound($"Progress record with ID {id} not found.");
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