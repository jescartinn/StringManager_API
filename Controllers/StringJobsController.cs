using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.Authorization;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StringJobsController : ControllerBase
{
    private readonly IStringJobService _stringJobService;

    public StringJobsController(IStringJobService stringJobService)
    {
        _stringJobService = stringJobService;
    }

    // GET: api/StringJobs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobs([FromQuery] string? status = null, [FromQuery] int? tournamentId = null)
    {
        IEnumerable<StringJobDto> stringJobs;

        if (!string.IsNullOrEmpty(status))
        {
            stringJobs = await _stringJobService.GetByStatusAsync(status);
        }
        else if (tournamentId.HasValue)
        {
            stringJobs = await _stringJobService.GetByTournamentIdAsync(tournamentId.Value);
        }
        else
        {
            stringJobs = await _stringJobService.GetAllAsync();
        }

        return Ok(stringJobs);
    }

    // GET: api/StringJobs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringJobDto>> GetStringJob(int id)
    {
        var stringJob = await _stringJobService.GetByIdAsync(id);

        if (stringJob == null)
        {
            return NotFound();
        }

        return Ok(stringJob);
    }

    // GET: api/StringJobs/player/5
    [HttpGet("player/{playerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByPlayer(int playerId)
    {
        var stringJobs = await _stringJobService.GetByPlayerIdAsync(playerId);

        // Si la lista está vacía, probablemente el jugador no existe
        if (!stringJobs.Any())
        {
            return NotFound("El jugador especificado no existe o no tiene trabajos de encordado.");
        }

        return Ok(stringJobs);
    }

    // GET: api/StringJobs/stringer/5
    [HttpGet("stringer/{stringerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByStringer(int stringerId)
    {
        var stringJobs = await _stringJobService.GetByStringerIdAsync(stringerId);

        // Si la lista está vacía, probablemente el encordador no existe
        if (!stringJobs.Any())
        {
            return NotFound("El encordador especificado no existe o no tiene trabajos de encordado asignados.");
        }

        return Ok(stringJobs);
    }

    // POST: api/StringJobs
    [HttpPost]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<ActionResult<StringJobDto>> CreateStringJob(CreateStringJobDto createStringJobDto)
    {
        try
        {
            var stringJob = await _stringJobService.CreateAsync(createStringJobDto);
            return CreatedAtAction(nameof(GetStringJob), new { id = stringJob.Id }, stringJob);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/StringJobs/5
    [HttpPut("{id}")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> UpdateStringJob(int id, UpdateStringJobDto updateStringJobDto)
    {
        var result = await _stringJobService.UpdateAsync(id, updateStringJobDto);

        if (!result)
        {
            return BadRequest("No se puede actualizar el trabajo. Puede que no exista o esté completado/cancelado.");
        }

        return NoContent();
    }

    // PATCH: api/StringJobs/5/complete
    [HttpPatch("{id}/complete")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> CompleteStringJob(int id, CompleteStringJobDto completeStringJobDto)
    {
        var result = await _stringJobService.CompleteJobAsync(id, completeStringJobDto);

        if (!result)
        {
            return BadRequest("No se puede completar el trabajo. Puede que no exista, ya esté completado o esté cancelado.");
        }

        return NoContent();
    }

    // PATCH: api/StringJobs/5/cancel
    [HttpPatch("{id}/cancel")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> CancelStringJob(int id, [FromBody] string? cancelReason)
    {
        var result = await _stringJobService.CancelJobAsync(id, cancelReason);

        if (!result)
        {
            return BadRequest("No se puede cancelar el trabajo. Puede que no exista, ya esté completado o esté cancelado.");
        }

        return NoContent();
    }

    // PATCH: api/StringJobs/5/start
    [HttpPatch("{id}/start")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> StartStringJob(int id)
    {
        var result = await _stringJobService.StartJobAsync(id);

        if (!result)
        {
            return BadRequest("No se puede iniciar el trabajo. Puede que no exista o no esté en estado pendiente.");
        }

        return NoContent();
    }

    // DELETE: api/StringJobs/5
    [HttpDelete("{id}")]
    [AuthorizeRoles("Admin")]
    public async Task<IActionResult> DeleteStringJob(int id)
    {
        var result = await _stringJobService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("Solo se pueden eliminar trabajos en estado pendiente.");
        }

        return NoContent();
    }
}