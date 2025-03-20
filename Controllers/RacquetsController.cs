using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacquetsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RacquetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Racquets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RacquetDto>>> GetRacquets([FromQuery] int? playerId = null)
    {
        IQueryable<Racquet> query = _context.Racquets.Include(r => r.Player);

        if (playerId.HasValue)
        {
            query = query.Where(r => r.PlayerId == playerId);
        }

        var racquets = await query.ToListAsync();

        var racquetDtos = racquets.Select(r => new RacquetDto
        {
            Id = r.Id,
            PlayerId = r.PlayerId,
            Brand = r.Brand,
            Model = r.Model,
            SerialNumber = r.SerialNumber,
            HeadSize = r.HeadSize,
            Notes = r.Notes,
            Player = r.Player != null ? new PlayerDto
            {
                Id = r.Player.Id,
                Name = r.Player.Name,
                LastName = r.Player.LastName,
                CountryCode = r.Player.CountryCode
            } : null
        }).ToList();

        return Ok(racquetDtos);
    }

    // GET: api/Racquets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RacquetDto>> GetRacquet(int id)
    {
        var racquet = await _context.Racquets
            .Include(r => r.Player)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (racquet == null)
        {
            return NotFound();
        }

        var racquetDto = new RacquetDto
        {
            Id = racquet.Id,
            PlayerId = racquet.PlayerId,
            Brand = racquet.Brand,
            Model = racquet.Model,
            SerialNumber = racquet.SerialNumber,
            HeadSize = racquet.HeadSize,
            Notes = racquet.Notes,
            Player = racquet.Player != null ? new PlayerDto
            {
                Id = racquet.Player.Id,
                Name = racquet.Player.Name,
                LastName = racquet.Player.LastName,
                CountryCode = racquet.Player.CountryCode
            } : null
        };

        return Ok(racquetDto);
    }

    // POST: api/Racquets
    [HttpPost]
    public async Task<ActionResult<RacquetDto>> CreateRacquet(CreateRacquetDto createRacquetDto)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == createRacquetDto.PlayerId);
        if (!playerExists)
        {
            return BadRequest("El jugador especificado no existe.");
        }

        var racquet = new Racquet
        {
            PlayerId = createRacquetDto.PlayerId,
            Brand = createRacquetDto.Brand,
            Model = createRacquetDto.Model,
            SerialNumber = createRacquetDto.SerialNumber,
            HeadSize = createRacquetDto.HeadSize,
            Notes = createRacquetDto.Notes
        };

        _context.Racquets.Add(racquet);
        await _context.SaveChangesAsync();

        // Cargar el jugador para el DTO de respuesta
        await _context.Entry(racquet)
            .Reference(r => r.Player)
            .LoadAsync();

        var racquetDto = new RacquetDto
        {
            Id = racquet.Id,
            PlayerId = racquet.PlayerId,
            Brand = racquet.Brand,
            Model = racquet.Model,
            SerialNumber = racquet.SerialNumber,
            HeadSize = racquet.HeadSize,
            Notes = racquet.Notes,
            Player = racquet.Player != null ? new PlayerDto
            {
                Id = racquet.Player.Id,
                Name = racquet.Player.Name,
                LastName = racquet.Player.LastName,
                CountryCode = racquet.Player.CountryCode
            } : null
        };

        return CreatedAtAction(nameof(GetRacquet), new { id = racquet.Id }, racquetDto);
    }

    // PUT: api/Racquets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRacquet(int id, UpdateRacquetDto updateRacquetDto)
    {
        var racquet = await _context.Racquets.FindAsync(id);

        if (racquet == null)
        {
            return NotFound();
        }

        racquet.Brand = updateRacquetDto.Brand;
        racquet.Model = updateRacquetDto.Model;
        racquet.SerialNumber = updateRacquetDto.SerialNumber;
        racquet.HeadSize = updateRacquetDto.HeadSize;
        racquet.Notes = updateRacquetDto.Notes;

        _context.Entry(racquet).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RacquetExists(id))
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

    // DELETE: api/Racquets/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRacquet(int id)
    {
        var racquet = await _context.Racquets.FindAsync(id);

        if (racquet == null)
        {
            return NotFound();
        }

        // Verificar si la raqueta tiene trabajos de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.RacquetId == id);

        if (hasStringJobs)
        {
            return BadRequest("No se puede eliminar la raqueta porque tiene trabajos de encordado asociados.");
        }

        _context.Racquets.Remove(racquet);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RacquetExists(int id)
    {
        return _context.Racquets.Any(e => e.Id == id);
    }
}