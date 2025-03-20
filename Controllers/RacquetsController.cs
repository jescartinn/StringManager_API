using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacquetsController : ControllerBase
{
    private readonly IRacquetService _racquetService;

    public RacquetsController(IRacquetService racquetService)
    {
        _racquetService = racquetService;
    }

    // GET: api/Racquets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RacquetDto>>> GetRacquets([FromQuery] int? playerId = null)
    {
        IEnumerable<RacquetDto> racquets;

        if (playerId.HasValue)
        {
            racquets = await _racquetService.GetByPlayerIdAsync(playerId.Value);
        }
        else
        {
            racquets = await _racquetService.GetAllAsync();
        }

        return Ok(racquets);
    }

    // GET: api/Racquets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RacquetDto>> GetRacquet(int id)
    {
        var racquet = await _racquetService.GetByIdAsync(id);

        if (racquet == null)
        {
            return NotFound();
        }

        return Ok(racquet);
    }

    // POST: api/Racquets
    [HttpPost]
    public async Task<ActionResult<RacquetDto>> CreateRacquet(CreateRacquetDto createRacquetDto)
    {
        try
        {
            var racquet = await _racquetService.CreateAsync(createRacquetDto);
            return CreatedAtAction(nameof(GetRacquet), new { id = racquet.Id }, racquet);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Racquets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRacquet(int id, UpdateRacquetDto updateRacquetDto)
    {
        var result = await _racquetService.UpdateAsync(id, updateRacquetDto);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Racquets/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRacquet(int id)
    {
        var result = await _racquetService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("No se puede eliminar la raqueta porque tiene trabajos de encordado asociados.");
        }

        return NoContent();
    }
}