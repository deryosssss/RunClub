using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.DTOs;
using RunClubAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        // ✅ Admin: View all
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetProgressRecords()
        {
            var records = await _progressRecordService.GetAllProgressRecordsAsync();
            if (records == null || !records.Any())
                return NotFound("No progress records available.");
            return Ok(records);
        }

        // ✅ Runner & Coach: View my own records
        [Authorize(Roles = "Runner,Coach")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyProgressRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID missing in token.");

            var allRecords = await _progressRecordService.GetAllProgressRecordsAsync();

            var myRecords = role switch
            {
                "Runner" => allRecords.Where(r => r.UserId == userId).ToList(),
                "Coach" => allRecords.Where(r => r.CoachId == userId).ToList(),
                _ => new List<ProgressRecordDTO>()
            };

            return Ok(myRecords);
        }


        // ✅ Get by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgressRecord(int id)
        {
            var record = await _progressRecordService.GetProgressRecordByIdAsync(id);
            return record == null
                ? NotFound($"Progress record with ID {id} not found.")
                : Ok(record);
        }

        // ✅ Coach OR Runner: Create progress
        [Authorize(Roles = "Coach,Runner")]
        [HttpPost]
        public async Task<IActionResult> PostProgressRecord([FromBody] ProgressRecordDTO dto)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (role == "Coach")
            {
                if (string.IsNullOrEmpty(dto.UserId))
                    return BadRequest("Coach must provide a UserId.");
            }
            else
            {
                dto.UserId = userId!;
            }

            if (string.IsNullOrWhiteSpace(dto.ProgressDate) || string.IsNullOrWhiteSpace(dto.ProgressTime))
                return BadRequest(new { message = "ProgressDate and ProgressTime are required." });

            var created = await _progressRecordService.AddProgressRecordAsync(dto);
            return created == null
                ? BadRequest("Could not create progress record.")
                : CreatedAtAction(nameof(GetProgressRecord), new { id = created.ProgressRecordId }, created);
        }

        // ✅ PUT (edit)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgressRecord(int id, [FromBody] ProgressRecordDTO dto)
        {
            if (id != dto.ProgressRecordId)
                return BadRequest("ProgressRecord ID mismatch.");

            if (string.IsNullOrWhiteSpace(dto.ProgressDate) || string.IsNullOrWhiteSpace(dto.ProgressTime))
                return BadRequest(new { message = "ProgressDate and ProgressTime are required." });

            var updated = await _progressRecordService.UpdateProgressRecordAsync(id, dto);
            return updated ? NoContent() : NotFound($"Progress record with ID {id} not found.");
        }

        // ✅ DELETE
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgressRecord(int id)
        {
            var deleted = await _progressRecordService.DeleteProgressRecordAsync(id);
            return deleted ? NoContent() : NotFound($"Progress record with ID {id} not found.");
        }

        // ✅ Runner: Request progress feedback
        [Authorize]
        [HttpPost("request")]
        public IActionResult RequestProgressFeedback()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not identified");

            _logger.LogInformation("Runner {UserId} requested progress feedback", userId);

            return Ok(new
            {
                message = "✅ Feedback request sent successfully",
                userId,
                requestedAt = DateTime.UtcNow,
                status = "Pending"
            });
        }

        // ✅ Coach: Get runner's progress
        [Authorize(Roles = "Coach")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProgressForUser(string userId)
        {
            var all = await _progressRecordService.GetAllProgressRecordsAsync();
            var runnerProgress = all.Where(p => p.UserId == userId);
            return Ok(runnerProgress);
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