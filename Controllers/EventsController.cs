using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunClubAPI.Models;
using Microsoft.AspNetCore.Authorization;


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
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest("Event ID mismatch.");
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
                    return NotFound();
                }
                return StatusCode(500, "A database error occurred while updating the event.");
            }

            return NoContent();
        }


        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEvent", new { id = newEvent.EventId }, newEvent);
        }


        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> EventExists(int id)
        {
            return await _context.Events.FindAsync(id) != null;
        }
    }
}

