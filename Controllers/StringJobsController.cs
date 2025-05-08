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

    [HttpGet("player/{playerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByPlayer(int playerId)
    {
        var stringJobs = await _stringJobService.GetByPlayerIdAsync(playerId);

        if (!stringJobs.Any())
        {
            return NotFound("El jugador especificado no existe o no tiene trabajos de encordado.");
        }

        return Ok(stringJobs);
    }

    [HttpGet("stringer/{stringerId}")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetStringJobsByStringer(int stringerId)
    {
        var stringJobs = await _stringJobService.GetByStringerIdAsync(stringerId);

        if (!stringJobs.Any())
        {
            return NotFound("El encordador especificado no existe o no tiene trabajos de encordado asignados.");
        }

        return Ok(stringJobs);
    }

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

    [HttpGet("player/{playerId}/unpaid")]
    public async Task<ActionResult<IEnumerable<StringJobDto>>> GetUnpaidStringJobsByPlayer(int playerId)
    {
        var stringJobs = await _stringJobService.GetUnpaidJobsByPlayerIdAsync(playerId);

        if (!stringJobs.Any())
        {
            return NotFound("El jugador especificado no existe o no tiene trabajos de encordado pendientes de pago.");
        }

        return Ok(stringJobs);
    }

    [HttpPatch("{id}/paid")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> MarkStringJobAsPaid(int id)
    {
        var result = await _stringJobService.MarkJobAsPaidAsync(id);

        if (!result)
        {
            return BadRequest("No se puede marcar el trabajo como pagado. Puede que no exista o no esté completado.");
        }

        return NoContent();
    }
}