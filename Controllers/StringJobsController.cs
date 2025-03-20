using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StringJobsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StringJobsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/StringJobs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobs([FromQuery] string? status = null, [FromQuery] int? tournamentId = null)
    {
        IQueryable<StringJob> query = _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament);

        // Filtrar por estado
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(sj => sj.Status == status);
        }

        // Filtrar por torneo
        if (tournamentId.HasValue)
        {
            query = query.Where(sj => sj.TournamentId == tournamentId);
        }

        // Ordenar por fecha de creación (los más recientes primero) y prioridad
        query = query.OrderByDescending(sj => sj.Priority)
                     .ThenByDescending(sj => sj.CreatedAt);

        var stringJobs = await query.ToListAsync();

        var stringJobDtos = stringJobs.Select(sj => MapToStringJobDto(sj)).ToList();

        return Ok(stringJobDtos);
    }

    // GET: api/StringJobs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringJobDto>> GetStringJob(int id)
    {
        var stringJob = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .FirstOrDefaultAsync(sj => sj.Id == id);

        if (stringJob == null)
        {
            return NotFound();
        }

        var stringJobDto = MapToStringJobDto(stringJob);

        return Ok(stringJobDto);
    }

    // POST: api/StringJobs
    [HttpPost]
    public async Task<ActionResult<StringJobDto>> CreateStringJob(CreateStringJobDto createStringJobDto)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == createStringJobDto.PlayerId);
        if (!playerExists)
        {
            return BadRequest("El jugador especificado no existe.");
        }

        // Verificar si la raqueta existe y pertenece al jugador
        var racquet = await _context.Racquets.FindAsync(createStringJobDto.RacquetId);
        if (racquet == null)
        {
            return BadRequest("La raqueta especificada no existe.");
        }
        if (racquet.PlayerId != createStringJobDto.PlayerId)
        {
            return BadRequest("La raqueta especificada no pertenece al jugador indicado.");
        }

        // Verificar las cuerdas si se proporcionan
        if (createStringJobDto.MainStringId.HasValue)
        {
            var mainStringExists = await _context.StringTypes.AnyAsync(st => st.Id == createStringJobDto.MainStringId);
            if (!mainStringExists)
            {
                return BadRequest("La cuerda principal especificada no existe.");
            }
        }

        if (createStringJobDto.CrossStringId.HasValue)
        {
            var crossStringExists = await _context.StringTypes.AnyAsync(st => st.Id == createStringJobDto.CrossStringId);
            if (!crossStringExists)
            {
                return BadRequest("La cuerda cruzada especificada no existe.");
            }
        }

        // Verificar el encordador si se proporciona
        if (createStringJobDto.StringerId.HasValue)
        {
            var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == createStringJobDto.StringerId);
            if (!stringerExists)
            {
                return BadRequest("El encordador especificado no existe.");
            }
        }

        // Verificar el torneo si se proporciona
        if (createStringJobDto.TournamentId.HasValue)
        {
            var tournamentExists = await _context.Tournaments.AnyAsync(t => t.Id == createStringJobDto.TournamentId);
            if (!tournamentExists)
            {
                return BadRequest("El torneo especificado no existe.");
            }
        }

        var stringJob = new StringJob
        {
            PlayerId = createStringJobDto.PlayerId,
            RacquetId = createStringJobDto.RacquetId,
            MainStringId = createStringJobDto.MainStringId,
            CrossStringId = createStringJobDto.CrossStringId,
            StringerId = createStringJobDto.StringerId,
            TournamentId = createStringJobDto.TournamentId,
            MainTension = createStringJobDto.MainTension,
            CrossTension = createStringJobDto.CrossTension,
            IsTensionInKg = createStringJobDto.IsTensionInKg,
            Notes = createStringJobDto.Notes,
            Priority = createStringJobDto.Priority,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _context.StringJobs.Add(stringJob);
        await _context.SaveChangesAsync();

        // Cargar las entidades relacionadas para el DTO de respuesta
        await _context.Entry(stringJob)
            .Reference(sj => sj.Player)
            .LoadAsync();

        await _context.Entry(stringJob)
            .Reference(sj => sj.Racquet)
            .LoadAsync();

        if (stringJob.MainStringId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.MainString)
                .LoadAsync();
        }

        if (stringJob.CrossStringId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.CrossString)
                .LoadAsync();
        }

        if (stringJob.StringerId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.Stringer)
                .LoadAsync();
        }

        if (stringJob.TournamentId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.Tournament)
                .LoadAsync();
        }

        var stringJobDto = MapToStringJobDto(stringJob);

        return CreatedAtAction(nameof(GetStringJob), new { id = stringJob.Id }, stringJobDto);
    }

    // PUT: api/StringJobs/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStringJob(int id, UpdateStringJobDto updateStringJobDto)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        // Verificar si el trabajo ya está completado
        if (stringJob.Status == "Completed" || stringJob.Status == "Cancelled")
        {
            return BadRequest("No se puede actualizar un trabajo que ya está completado o cancelado.");
        }

        // Verificar las cuerdas si se proporcionan
        if (updateStringJobDto.MainStringId.HasValue)
        {
            var mainStringExists = await _context.StringTypes.AnyAsync(st => st.Id == updateStringJobDto.MainStringId);
            if (!mainStringExists)
            {
                return BadRequest("La cuerda principal especificada no existe.");
            }
            stringJob.MainStringId = updateStringJobDto.MainStringId;
        }

        if (updateStringJobDto.CrossStringId.HasValue)
        {
            var crossStringExists = await _context.StringTypes.AnyAsync(st => st.Id == updateStringJobDto.CrossStringId);
            if (!crossStringExists)
            {
                return BadRequest("La cuerda cruzada especificada no existe.");
            }
            stringJob.CrossStringId = updateStringJobDto.CrossStringId;
        }

        // Verificar el encordador si se proporciona
        if (updateStringJobDto.StringerId.HasValue)
        {
            var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == updateStringJobDto.StringerId);
            if (!stringerExists)
            {
                return BadRequest("El encordador especificado no existe.");
            }
            stringJob.StringerId = updateStringJobDto.StringerId;
        }

        stringJob.MainTension = updateStringJobDto.MainTension;
        stringJob.CrossTension = updateStringJobDto.CrossTension;
        stringJob.IsTensionInKg = updateStringJobDto.IsTensionInKg;
        stringJob.Status = updateStringJobDto.Status;
        stringJob.Notes = updateStringJobDto.Notes;
        stringJob.Priority = updateStringJobDto.Priority;

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringJobExists(id))
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

    // PATCH: api/StringJobs/5/complete
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteStringJob(int id, CompleteStringJobDto completeStringJobDto)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        // Verificar si el trabajo ya está completado o cancelado
        if (stringJob.Status == "Completed")
        {
            return BadRequest("Este trabajo ya está completado.");
        }

        if (stringJob.Status == "Cancelled")
        {
            return BadRequest("No se puede completar un trabajo cancelado.");
        }

        stringJob.Status = "Completed";
        stringJob.CompletedAt = completeStringJobDto.CompletedAt;

        if (!string.IsNullOrEmpty(completeStringJobDto.Notes))
        {
            stringJob.Notes = (string.IsNullOrEmpty(stringJob.Notes))
                ? completeStringJobDto.Notes
                : $"{stringJob.Notes}\n\nNotas de finalización: {completeStringJobDto.Notes}";
        }

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringJobExists(id))
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

    // PATCH: api/StringJobs/5/cancel
    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelStringJob(int id, [FromBody] string? cancelReason)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        // Verificar si el trabajo ya está completado o cancelado
        if (stringJob.Status == "Completed")
        {
            return BadRequest("No se puede cancelar un trabajo que ya está completado.");
        }

        if (stringJob.Status == "Cancelled")
        {
            return BadRequest("Este trabajo ya está cancelado.");
        }

        stringJob.Status = "Cancelled";

        if (!string.IsNullOrEmpty(cancelReason))
        {
            stringJob.Notes = (string.IsNullOrEmpty(stringJob.Notes))
                ? $"Cancelado: {cancelReason}"
                : $"{stringJob.Notes}\n\nCancelado: {cancelReason}";
        }

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringJobExists(id))
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

    // PATCH: api/StringJobs/5/start
    [HttpPatch("{id}/start")]
    public async Task<IActionResult> StartStringJob(int id)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        // Solo se pueden iniciar trabajos pendientes
        if (stringJob.Status != "Pending")
        {
            return BadRequest($"No se puede iniciar un trabajo con estado '{stringJob.Status}'.");
        }

        stringJob.Status = "InProgress";

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringJobExists(id))
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

    // GET: api/StringJobs/player/5
    [HttpGet("player/{playerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByPlayer(int playerId)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == playerId);
        if (!playerExists)
        {
            return NotFound("El jugador especificado no existe.");
        }

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.PlayerId == playerId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        var stringJobDtos = stringJobs.Select(sj => MapToStringJobDto(sj)).ToList();

        return Ok(stringJobDtos);
    }

    // GET: api/StringJobs/tournament/5
    [HttpGet("tournament/{tournamentId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByTournament(int tournamentId)
    {
        // Verificar si el torneo existe
        var tournamentExists = await _context.Tournaments.AnyAsync(t => t.Id == tournamentId);
        if (!tournamentExists)
        {
            return NotFound("El torneo especificado no existe.");
        }

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.TournamentId == tournamentId)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        var stringJobDtos = stringJobs.Select(sj => MapToStringJobDto(sj)).ToList();

        return Ok(stringJobDtos);
    }

    // GET: api/StringJobs/stringer/5
    [HttpGet("stringer/{stringerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByStringer(int stringerId)
    {
        // Verificar si el encordador existe
        var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == stringerId);
        if (!stringerExists)
        {
            return NotFound("El encordador especificado no existe.");
        }

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.StringerId == stringerId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        var stringJobDtos = stringJobs.Select(sj => MapToStringJobDto(sj)).ToList();

        return Ok(stringJobDtos);
    }

    // DELETE: api/StringJobs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStringJob(int id)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        // Solo permitir eliminar trabajos en estado Pending
        if (stringJob.Status != "Pending")
        {
            return BadRequest("Solo se pueden eliminar trabajos en estado pendiente.");
        }

        _context.StringJobs.Remove(stringJob);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StringJobExists(int id)
    {
        return _context.StringJobs.Any(e => e.Id == id);
    }

    private StringJobDto MapToStringJobDto(StringJob stringJob)
    {
        return new StringJobDto
        {
            Id = stringJob.Id,
            PlayerId = stringJob.PlayerId,
            Player = stringJob.Player != null ? new PlayerDto
            {
                Id = stringJob.Player.Id,
                Name = stringJob.Player.Name,
                LastName = stringJob.Player.LastName,
                CountryCode = stringJob.Player.CountryCode
            } : null,
            RacquetId = stringJob.RacquetId,
            Racquet = stringJob.Racquet != null ? new RacquetDto
            {
                Id = stringJob.Racquet.Id,
                PlayerId = stringJob.Racquet.PlayerId,
                Brand = stringJob.Racquet.Brand,
                Model = stringJob.Racquet.Model,
                SerialNumber = stringJob.Racquet.SerialNumber,
                HeadSize = stringJob.Racquet.HeadSize,
                Notes = stringJob.Racquet.Notes
            } : null,
            MainStringId = stringJob.MainStringId,
            MainString = stringJob.MainString != null ? new StringTypeDto
            {
                Id = stringJob.MainString.Id,
                Brand = stringJob.MainString.Brand,
                Model = stringJob.MainString.Model,
                Gauge = stringJob.MainString.Gauge,
                Material = stringJob.MainString.Material,
                Color = stringJob.MainString.Color
            } : null,
            CrossStringId = stringJob.CrossStringId,
            CrossString = stringJob.CrossString != null ? new StringTypeDto
            {
                Id = stringJob.CrossString.Id,
                Brand = stringJob.CrossString.Brand,
                Model = stringJob.CrossString.Model,
                Gauge = stringJob.CrossString.Gauge,
                Material = stringJob.CrossString.Material,
                Color = stringJob.CrossString.Color
            } : null,
            StringerId = stringJob.StringerId,
            Stringer = stringJob.Stringer != null ? new StringerDto
            {
                Id = stringJob.Stringer.Id,
                Name = stringJob.Stringer.Name,
                LastName = stringJob.Stringer.LastName,
                Email = stringJob.Stringer.Email,
                PhoneNumber = stringJob.Stringer.PhoneNumber
            } : null,
            TournamentId = stringJob.TournamentId,
            Tournament = stringJob.Tournament != null ? new TournamentDto
            {
                Id = stringJob.Tournament.Id,
                Name = stringJob.Tournament.Name,
                StartDate = stringJob.Tournament.StartDate,
                EndDate = stringJob.Tournament.EndDate,
                Location = stringJob.Tournament.Location,
                Category = stringJob.Tournament.Category
            } : null,
            CreatedAt = stringJob.CreatedAt,
            CompletedAt = stringJob.CompletedAt,
            MainTension = stringJob.MainTension,
            CrossTension = stringJob.CrossTension,
            IsTensionInKg = stringJob.IsTensionInKg,
            Status = stringJob.Status,
            Notes = stringJob.Notes,
            Priority = stringJob.Priority
        };
    }
}