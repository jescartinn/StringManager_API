using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PlayersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
    {
        var players = await _context.Players.ToListAsync();

        var playerDtos = players.Select(p => new PlayerDto
        {
            Id = p.Id,
            Name = p.Name,
            LastName = p.LastName,
            CountryCode = p.CountryCode
        }).ToList();

        return Ok(playerDtos);
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        var playerDto = new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            LastName = player.LastName,
            CountryCode = player.CountryCode
        };

        return Ok(playerDto);
    }

    // POST: api/Players
    [HttpPost]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        var player = new Player
        {
            Name = createPlayerDto.Name,
            LastName = createPlayerDto.LastName,
            CountryCode = createPlayerDto.CountryCode
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        var playerDto = new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            LastName = player.LastName,
            CountryCode = player.CountryCode
        };

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, playerDto);
    }

    // PUT: api/Players/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerDto updatePlayerDto)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        player.Name = updatePlayerDto.Name;
        player.LastName = updatePlayerDto.LastName;
        player.CountryCode = updatePlayerDto.CountryCode;

        _context.Entry(player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlayerExists(id))
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

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        // Verificar si el jugador tiene trabajos de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.PlayerId == id);

        if (hasStringJobs)
        {
            return BadRequest("No se puede eliminar el jugador porque tiene trabajos de encordado asociados.");
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlayerExists(int id)
    {
        return _context.Players.Any(e => e.Id == id);
    }
}