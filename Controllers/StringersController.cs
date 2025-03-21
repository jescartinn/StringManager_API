using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.Authorization;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StringersController : ControllerBase
{
    private readonly IStringerService _stringerService;

    public StringersController(IStringerService stringerService)
    {
        _stringerService = stringerService;
    }

    // GET: api/Stringers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringerDto>>> GetStringers()
    {
        var stringers = await _stringerService.GetAllAsync();
        return Ok(stringers);
    }

    // GET: api/Stringers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringerDto>> GetStringer(int id)
    {
        var stringer = await _stringerService.GetByIdAsync(id);

        if (stringer == null)
        {
            return NotFound();
        }

        return Ok(stringer);
    }

    // POST: api/Stringers
    [HttpPost]
    [AuthorizeRoles("Admin")]
    public async Task<ActionResult<StringerDto>> CreateStringer(CreateStringerDto createStringerDto)
    {
        var stringer = await _stringerService.CreateAsync(createStringerDto);
        return CreatedAtAction(nameof(GetStringer), new { id = stringer.Id }, stringer);
    }

    // PUT: api/Stringers/5
    [HttpPut("{id}")]
    [AuthorizeRoles("Admin")]
    public async Task<IActionResult> UpdateStringer(int id, UpdateStringerDto updateStringerDto)
    {
        var result = await _stringerService.UpdateAsync(id, updateStringerDto);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Stringers/5
    [HttpDelete("{id}")]
    [AuthorizeRoles("Admin")]
    public async Task<IActionResult> DeleteStringer(int id)
    {
        var result = await _stringerService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("No se puede eliminar el encordador porque está asignado a trabajos de encordado.");
        }

        return NoContent();
    }
}