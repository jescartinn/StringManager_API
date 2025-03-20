using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StringTypesController : ControllerBase
{
    private readonly IStringTypeService _stringTypeService;

    public StringTypesController(IStringTypeService stringTypeService)
    {
        _stringTypeService = stringTypeService;
    }

    // GET: api/StringTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringTypeDto>>> GetStringTypes()
    {
        var stringTypes = await _stringTypeService.GetAllAsync();
        return Ok(stringTypes);
    }

    // GET: api/StringTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringTypeDto>> GetStringType(int id)
    {
        var stringType = await _stringTypeService.GetByIdAsync(id);

        if (stringType == null)
        {
            return NotFound();
        }

        return Ok(stringType);
    }

    // POST: api/StringTypes
    [HttpPost]
    public async Task<ActionResult<StringTypeDto>> CreateStringType(CreateStringTypeDto createStringTypeDto)
    {
        var stringType = await _stringTypeService.CreateAsync(createStringTypeDto);
        return CreatedAtAction(nameof(GetStringType), new { id = stringType.Id }, stringType);
    }

    // PUT: api/StringTypes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStringType(int id, UpdateStringTypeDto updateStringTypeDto)
    {
        var result = await _stringTypeService.UpdateAsync(id, updateStringTypeDto);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/StringTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStringType(int id)
    {
        var result = await _stringTypeService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest("No se puede eliminar el tipo de cuerda porque está siendo utilizado en trabajos de encordado.");
        }

        return NoContent();
    }
}