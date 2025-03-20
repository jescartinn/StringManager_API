using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentsController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
    {
        var tournaments = await _tournamentService.GetAllAsync();
        return Ok(tournaments);
    }

    // GET: api/Tournaments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        return Ok(tournament);
    }

    // GET: api/Tournaments/current
    [HttpGet("current")]
    public async Task<ActionResult<TournamentDto>> GetCurrentTournament()
    {
        var tournament = await _tournamentService.GetCurrentTournamentAsync();

        if (tournament == null)
        {
            return NotFound("No hay torneos activos en la fecha actual.");
        }

        return Ok(tournament);
    }

    // POST: api/Tournaments
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> CreateTournament(CreateTournamentDto createTournamentDto)
    {
        try
        {
            var tournament = await _tournamentService.CreateAsync(createTournamentDto);
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Tournaments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTournament(int id, UpdateTournamentDto updateTournamentDto)
    {
        try
        {
            var result = await _tournamentService.UpdateAsync(id, updateTournamentDto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(int id)
    {
        var result = await _tournamentService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("No se puede eliminar el torneo porque está asociado a trabajos de encordado.");
        }

        return NoContent();
    }
}