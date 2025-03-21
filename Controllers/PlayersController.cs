using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.Authorization;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
    {
        var players = await _playerService.GetAllAsync();
        return Ok(players);
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
    {
        var player = await _playerService.GetByIdAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    // POST: api/Players
    [HttpPost]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        var player = await _playerService.CreateAsync(createPlayerDto);
        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    // PUT: api/Players/5
    [HttpPut("{id}")]
    [AuthorizeRoles("Admin", "Stringer")]
    public async Task<IActionResult> UpdatePlayer(int id, UpdatePlayerDto updatePlayerDto)
    {
        var result = await _playerService.UpdateAsync(id, updatePlayerDto);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    [AuthorizeRoles("Admin")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var result = await _playerService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("No se puede eliminar el jugador porque tiene trabajos de encordado asociados.");
        }

        return NoContent();
    }
}