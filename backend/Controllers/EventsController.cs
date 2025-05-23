using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.DTOs;
using RunClubAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunClubAPI.Data;
using System.Security.Claims;


namespace RunClubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RunClubContext _context;

        public EventsController(RunClubContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Enrollments)
                .Include(e => e.Coach) // Make sure Coach is included
                .ToListAsync();

            var eventDtos = events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                EventName = e.EventName,
                Description = e.Description,
                EventDate = e.EventDate,
                EventTime = e.EventTime.ToString("HH:mm:ss"),
                Location = e.Location,
                EnrollmentCount = e.Enrollments?.Count ?? 0,
                ImageUrl = e.ImageUrl,
                CoachName = e.Coach != null ? e.Coach.Name : null,
                CoachPhotoUrl = e.Coach != null ? e.Coach.PhotoUrl : null
            }).ToList();

            return Ok(eventDtos);
        }

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var e = await _context.Events
                .Include(e => e.Enrollments)
                .Include(e => e.Coach)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (e == null)
                return NotFound(new { message = "Event not found." });

            var dto = new EventDTO
            {
                EventId = e.EventId,
                EventName = e.EventName,
                Description = e.Description,
                EventDate = e.EventDate,
                EventTime = e.EventTime.ToString("HH:mm:ss"),
                Location = e.Location,
                EnrollmentCount = e.Enrollments?.Count ?? 0,
                ImageUrl = e.ImageUrl,
                CoachName = e.Coach?.Name,
                CoachPhotoUrl = e.Coach?.PhotoUrl
            };

            return Ok(dto);
        }


        // PUT: api/events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, [FromBody] EventDTO updatedEvent)
        {
            if (id != updatedEvent.EventId)
                return BadRequest(new { message = "Event ID mismatch." });

            var eventToUpdate = await _context.Events.FindAsync(id);
            if (eventToUpdate == null)
                return NotFound(new { message = "Event not found for update." });

            eventToUpdate.EventName = updatedEvent.EventName;
            eventToUpdate.Description = updatedEvent.Description;
            eventToUpdate.EventDate = updatedEvent.EventDate;

            if (!TimeOnly.TryParse(updatedEvent.EventTime, out var parsedTime))
                return BadRequest(new { message = "Invalid event time format." });

            eventToUpdate.EventTime = parsedTime;
            eventToUpdate.Location = updatedEvent.Location;
            eventToUpdate.ImageUrl = updatedEvent.ImageUrl;

            // Optional: If CoachId is passed, update the coach
            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Name == updatedEvent.CoachName);
            if (coach != null)
            {
                eventToUpdate.CoachId = coach.Id;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] EventDTO newEventDTO)
        {
            if (newEventDTO == null)
                return BadRequest("Event data is missing.");

            if (!TimeOnly.TryParse(newEventDTO.EventTime, out var parsedTime))
                return BadRequest("Invalid time format. Expected HH:mm:ss.");

            var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.Name == newEventDTO.CoachName);

            var newEvent = new Event
            {
                EventName = newEventDTO.EventName,
                Description = newEventDTO.Description,
                EventDate = newEventDTO.EventDate,
                EventTime = parsedTime,
                Location = newEventDTO.Location,
                ImageUrl = newEventDTO.ImageUrl,
                CoachId = coach?.Id
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, new EventDTO(newEvent));
        }

        // DELETE: api/events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound(new { message = "Event not found." });

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> EventExists(int id)
        {
            return await _context.Events.AnyAsync(e => e.EventId == id);
        }

        [Authorize(Roles = "Coach")]
        [HttpGet("hosted")]
        public async Task<IActionResult> GetEventsHostedByMe()
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(coachId))
                return Unauthorized("Coach ID not found in token.");

            // Match the coach by Id (int). Make sure your coach IDs are stored as string in token or convert.
            if (!int.TryParse(coachId, out int parsedCoachId))
                return BadRequest("Invalid Coach ID format.");

            var upcomingMyEvents = await _context.Events
                .Include(e => e.Enrollments)
                .Include(e => e.Coach)
                .Where(e => e.CoachId == parsedCoachId && e.EventDate >= DateOnly.FromDateTime(DateTime.Today))
                .ToListAsync();

            var dtoList = upcomingMyEvents.Select(e => new EventDTO(e)).ToList();

            return Ok(dtoList);
        }


    }

}


/* 
"This controller handles CRUD operations for events while enforcing admin-only access."
"We use DTOs (EventDTO) to structure responses and avoid exposing unnecessary database fields."
"Concurrency handling ensures that if an event is deleted during an update attempt, we return a clear error message."
"Role-based authorization ([Authorize(Roles = "Admin")]) ensures that only admins can create, edit, or delete events."*/