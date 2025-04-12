using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RunClubAPI.DTOs;
using RunClubAPI.Models;
using RunClubAPI.Services;
using RunClubAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CoachesController : ControllerBase
{
    private readonly ICoachService _coachService;

    public CoachesController(ICoachService coachService)
    {
        _coachService = coachService;
    }

    // ✅ GET: api/coaches
    [HttpGet]
    public ActionResult<IEnumerable<CoachDto>> GetAll()
    {
        return Ok(_coachService.GetAllCoaches());
    }

    // ✅ GET: api/coaches/5
    [HttpGet("{id}")]
    public ActionResult<CoachDto> GetById(int id)
    {
        var coach = _coachService.GetCoachById(id);
        if (coach == null)
            return NotFound();

        return Ok(coach);
    }

    // ✅ POST: api/coaches
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult<CoachDto> Create([FromBody] CoachDto coachDto)
    {
        var createdCoach = _coachService.CreateCoach(coachDto);
        return CreatedAtAction(nameof(GetById), new { id = createdCoach.Id }, createdCoach);
    }

    // ✅ PUT: api/coaches/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CoachDto coachDto)
    {
        var updated = _coachService.UpdateCoach(id, coachDto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    // ✅ DELETE: api/coaches/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _coachService.DeleteCoach(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
