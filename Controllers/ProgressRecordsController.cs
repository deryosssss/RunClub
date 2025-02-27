using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Coach")] // Restricts access to users with "Coach" role
    public class ProgressRecordsController : ControllerBase
    {
        private readonly IProgressRecordService _progressRecordService;
        private readonly ILogger<ProgressRecordsController> _logger;

        public ProgressRecordsController(IProgressRecordService progressRecordService, ILogger<ProgressRecordsController> logger)
        {
            _progressRecordService = progressRecordService;
            _logger = logger;
        }

        // ✅ GET: api/ProgressRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressRecordDTO>>> GetProgressRecords()
        {
            _logger.LogInformation("Fetching all progress records...");
            var progressRecords = await _progressRecordService.GetAllProgressRecordsAsync();
            
            if (progressRecords?.Any() != true) // ✅ Null-safe check for empty list
            {
                _logger.LogWarning("No progress records found.");
                return NotFound("No progress records available.");
            }


            return Ok(progressRecords);
        }

        // ✅ GET: api/ProgressRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressRecordDTO>> GetProgressRecord(int id)
        {
            _logger.LogInformation($"Fetching progress record with ID {id}");
            var progressRecord = await _progressRecordService.GetProgressRecordByIdAsync(id);

            if (progressRecord == null)
            {
                _logger.LogWarning($"Progress record with ID {id} not found.");
                return NotFound($"Progress record with ID {id} not found.");
            }

            return Ok(progressRecord);
        }

        // ✅ POST: api/ProgressRecords
        [HttpPost]
        public async Task<ActionResult<ProgressRecordDTO>> PostProgressRecord(ProgressRecordDTO progressRecordDto)
        {
            _logger.LogInformation("Attempting to add a new progress record...");

            // Ensure progressDate and progressTime are properly formatted
            if (string.IsNullOrWhiteSpace(progressRecordDto.ProgressDate) || string.IsNullOrWhiteSpace(progressRecordDto.ProgressTime))
            {
                return BadRequest(new { message = "Both ProgressDate and ProgressTime are required." });
            }

            var createdRecord = await _progressRecordService.AddProgressRecordAsync(progressRecordDto);

            if (createdRecord == null)
            {
                return BadRequest(new { message = "Invalid User ID. Cannot add progress record." });
            }

            return CreatedAtAction(nameof(GetProgressRecord), new { id = createdRecord.ProgressRecordId }, createdRecord);
        }

        // ✅ PUT: api/ProgressRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgressRecord(int id, ProgressRecordDTO progressRecordDto)
        {
            if (id != progressRecordDto.ProgressRecordId)
            {
                return BadRequest("ProgressRecord ID mismatch.");
            }

            _logger.LogInformation($"Updating progress record with ID {id}");

            // Ensure progressDate and progressTime are properly formatted
            if (string.IsNullOrWhiteSpace(progressRecordDto.ProgressDate) || string.IsNullOrWhiteSpace(progressRecordDto.ProgressTime))
            {
                return BadRequest(new { message = "Both ProgressDate and ProgressTime are required." });
            }

            var updateSuccess = await _progressRecordService.UpdateProgressRecordAsync(id, progressRecordDto);

            if (!updateSuccess)
            {
                return NotFound($"Progress record with ID {id} not found.");
            }

            return NoContent();
        }

        // ✅ DELETE: api/ProgressRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgressRecord(int id)
        {
            _logger.LogInformation($"Attempting to delete progress record with ID {id}");
            var deleteSuccess = await _progressRecordService.DeleteProgressRecordAsync(id);

            if (!deleteSuccess)
            {
                return NotFound($"Progress record with ID {id} not found.");
            }

            return NoContent();
        }
    }
}

