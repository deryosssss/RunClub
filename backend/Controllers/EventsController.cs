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
    // This controller manages CRUD operations for events.
    [Route("api/[controller]")] // Base URL: "api/events"
    [ApiController] // Enables model binding, automatic validation, and more.
    [Authorize(Roles = "Admin")] // Only Admin users can access these endpoints.
    public class EventsController : ControllerBase
    {
        private readonly RunClubContext _context; // Injected database context.

        // Constructor: Injects the database context.
        public EventsController(RunClubContext context)
        {
            _context = context;
        }

        // GET all events (Includes enrollments count)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            // Fetch all events with their enrollments count.
            var events = await _context.Events
                .Include(e => e.Enrollments) // Joins enrollments data.
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    EventTime = e.EventTime.ToString("HH:mm:ss"),
                    Location = e.Location,
                    EnrollmentCount = e.Enrollments.Count()
                })
                .ToListAsync();

            return Ok(events); // Returns a list of event DTOs.
        }

        // GET a single event by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            // Fetch a single event with enrollments count.
            var eventItem = await _context.Events
                .Include(e => e.Enrollments)
                .Where(e => e.EventId == id)
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    EventTime = e.EventTime.ToString("HH:mm:ss"), 
                    Location = e.Location,
                    EnrollmentCount = e.Enrollments.Count()
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found." }); //  Returns 404 if event doesn't exist.
            }

            return Ok(eventItem); // Returns the event DTO.
        }

        // UPDATE an existing event
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest(new { message = "Event ID mismatch." }); // Ensures ID in URL matches event object.
            }

            _context.Entry(@event).State = EntityState.Modified; // Marks entity as modified.

            try
            {
                await _context.SaveChangesAsync(); // Saves changes to the database.
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(id)) // Checks if event still exists.
                {
                    return NotFound(new { message = "Event not found for update." });
                }
                return StatusCode(500, new { message = "A database error occurred while updating the event." });
            }

            return NoContent(); // 204 No Content (Successful update).
        }

        // CREATE a new event
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(EventDTO newEventDTO)
        {
            if (newEventDTO == null)
            {
                return BadRequest("Invalid event data."); // Ensures request is not empty.
            }

            // Converts EventDTO to Event model.
            var newEvent = new Event
            {
                EventName = newEventDTO.EventName,
                Description = newEventDTO.Description,
                EventDate = newEventDTO.EventDate,
                EventTime = TimeOnly.Parse(newEventDTO.EventTime), // Converts string to TimeOnly.
                Location = newEventDTO.Location
            };

            _context.Events.Add(newEvent); // Adds new event to the database.
            await _context.SaveChangesAsync(); // Saves the event.

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEvent); // 201 Created response.
        }

        // DELETE an event by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found." }); // Returns 404 if event doesn’t exist.
            }

            _context.Events.Remove(eventItem); // Removes the event.
            await _context.SaveChangesAsync(); // Saves the deletion.

            return NoContent(); // 204 No Content (Successful deletion).
        }

        // Helper method to check if an event exists.
        private async Task<bool> EventExists(int id)
        {
            return await _context.Events.AnyAsync(e => e.EventId == id); // Checks existence.
        }
    }
}

/* 
"This controller handles CRUD operations for events while enforcing admin-only access."
"We use DTOs (EventDTO) to structure responses and avoid exposing unnecessary database fields."
"Concurrency handling ensures that if an event is deleted during an update attempt, we return a clear error message."
"Role-based authorization ([Authorize(Roles = "Admin")]) ensures that only admins can create, edit, or delete events."*/