using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.DTOs;
using RunClubAPI.Models;

namespace RunClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EventsController : ControllerBase
    {
        private readonly RunClubContext _context;

        public EventsController(RunClubContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Enrollments)
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    EventTime = e.EventTime.ToString("HH:mm:ss"), // ✅ Fix conversion
                    Location = e.Location,
                    EnrollmentCount = e.Enrollments.Count()
                })
                .ToListAsync();

            return Ok(events);
        }


        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Enrollments)
                .Where(e => e.EventId == id)
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    EventTime = e.EventTime.ToString("HH:mm:ss"), // ✅ Fix conversion
                    Location = e.Location,
                    EnrollmentCount = e.Enrollments.Count()
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            return Ok(eventItem);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest(new { message = "Event ID mismatch." });
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(id))
                {
                    return NotFound(new { message = "Event not found for update." });
                }
                return StatusCode(500, new { message = "A database error occurred while updating the event." });
            }

            return NoContent();
        }


                // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventDTO newEventDTO)
        {
            if (newEventDTO == null)
            {
                return BadRequest("Invalid event data.");
            }

            // Convert EventDTO to Event
            var newEvent = new Event
            {
                EventName = newEventDTO.EventName,
                Description = newEventDTO.Description,
                EventDate = newEventDTO.EventDate,
                EventTime = TimeOnly.Parse(newEventDTO.EventTime), // Convert string to TimeOnly
                Location = newEventDTO.Location
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEvent);
        }
      



        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> EventExists(int id)
        {
            return await _context.Events.AnyAsync(e => e.EventId == id);
        }
    }
}
