using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TournamentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournaments()
    {
        var tournaments = await _context.Tournaments.ToListAsync();

        var tournamentDtos = tournaments.Select(t => new TournamentDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Location = t.Location,
            Category = t.Category
        }).ToList();

        return Ok(tournamentDtos);
    }

    // GET: api/Tournaments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id)
    {
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        var tournamentDto = new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Category = tournament.Category
        };

        return Ok(tournamentDto);
    }

    // GET: api/Tournaments/current
    [HttpGet("current")]
    public async Task<ActionResult<TournamentDto>> GetCurrentTournament()
    {
        var currentDate = DateTime.Now.Date;

        var tournament = await _context.Tournaments
            .Where(t => t.StartDate <= currentDate && t.EndDate >= currentDate)
            .OrderBy(t => t.StartDate)
            .FirstOrDefaultAsync();

        if (tournament == null)
        {
            return NotFound("No hay torneos activos en la fecha actual.");
        }

        var tournamentDto = new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Category = tournament.Category
        };

        return Ok(tournamentDto);
    }

    // POST: api/Tournaments
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> CreateTournament(CreateTournamentDto createTournamentDto)
    {
        // Validar fechas
        if (createTournamentDto.EndDate < createTournamentDto.StartDate)
        {
            return BadRequest("La fecha de finalización no puede ser anterior a la fecha de inicio.");
        }

        var tournament = new Tournament
        {
            Name = createTournamentDto.Name,
            StartDate = createTournamentDto.StartDate,
            EndDate = createTournamentDto.EndDate,
            Location = createTournamentDto.Location,
            Category = createTournamentDto.Category
        };

        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        var tournamentDto = new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Category = tournament.Category
        };

        return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournamentDto);
    }

    // PUT: api/Tournaments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTournament(int id, UpdateTournamentDto updateTournamentDto)
    {
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        // Validar fechas
        if (updateTournamentDto.EndDate < updateTournamentDto.StartDate)
        {
            return BadRequest("La fecha de finalización no puede ser anterior a la fecha de inicio.");
        }

        tournament.Name = updateTournamentDto.Name;
        tournament.StartDate = updateTournamentDto.StartDate;
        tournament.EndDate = updateTournamentDto.EndDate;
        tournament.Location = updateTournamentDto.Location;
        tournament.Category = updateTournamentDto.Category;

        _context.Entry(tournament).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TournamentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(int id)
    {
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
        {
            return NotFound();
        }

        // Verificar si el torneo está asociado a algún trabajo de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.TournamentId == id);

        if (hasStringJobs)
        {
            return BadRequest("No se puede eliminar el torneo porque está asociado a trabajos de encordado.");
        }

        _context.Tournaments.Remove(tournament);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TournamentExists(int id)
    {
        return _context.Tournaments.Any(e => e.Id == id);
    }
}