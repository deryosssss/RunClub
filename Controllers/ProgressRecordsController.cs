using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Models;

namespace RunClub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Coach")] // Restricts access to users with "Coach" role
    public class ProgressRecordsController : ControllerBase
    {
        private readonly RunClubContext _context;

        public ProgressRecordsController(RunClubContext context)
        {
            _context = context;
        }

        // GET: api/ProgressRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressRecord>>> GetProgressRecords()
        {
            return await _context.ProgressRecords.ToListAsync();
        }

        // GET: api/ProgressRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressRecord>> GetProgressRecord(int id)
        {
            var progressRecord = await _context.ProgressRecords.FindAsync(id);

            if (progressRecord == null)
            {
                return NotFound($"Progress record with ID {id} not found.");
            }

            return Ok(progressRecord);
        }

        // PUT: api/ProgressRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgressRecord(int id, ProgressRecord progress)
        {
            if (id != progress.ProgressRecordId)
            {
                return BadRequest("ProgressRecord ID mismatch.");
            }

            _context.Entry(progress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProgressRecordExists(id))
                {
                    return NotFound($"Progress record with ID {id} not found.");
                }
                return StatusCode(500, "A database error occurred while updating the record.");
            }

            return NoContent();
        }

        // POST: api/ProgressRecords
        [HttpPost]
        public async Task<ActionResult<ProgressRecord>> PostProgressRecord(ProgressRecord progressRecord)
        {
            _context.ProgressRecords.Add(progressRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProgressRecord), new { id = progressRecord.ProgressRecordId }, progressRecord);
        }

        // DELETE: api/ProgressRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgressRecord(int id)
        {
            var progressRecord = await _context.ProgressRecords.FindAsync(id);
            if (progressRecord == null)
            {
                return NotFound($"Progress record with ID {id} not found.");
            }

            _context.ProgressRecords.Remove(progressRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ProgressRecordExists(int id)
        {
            return await _context.ProgressRecords.AnyAsync(e => e.ProgressRecordId == id);
        }
    }
}
