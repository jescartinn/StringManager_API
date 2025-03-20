using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StringTypesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StringTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/StringTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringTypeDto>>> GetStringTypes()
    {
        var stringTypes = await _context.StringTypes.ToListAsync();

        var stringTypeDtos = stringTypes.Select(st => new StringTypeDto
        {
            Id = st.Id,
            Brand = st.Brand,
            Model = st.Model,
            Gauge = st.Gauge,
            Material = st.Material,
            Color = st.Color
        }).ToList();

        return Ok(stringTypeDtos);
    }

    // GET: api/StringTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringTypeDto>> GetStringType(int id)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
        {
            return NotFound();
        }

        var stringTypeDto = new StringTypeDto
        {
            Id = stringType.Id,
            Brand = stringType.Brand,
            Model = stringType.Model,
            Gauge = stringType.Gauge,
            Material = stringType.Material,
            Color = stringType.Color
        };

        return Ok(stringTypeDto);
    }

    // POST: api/StringTypes
    [HttpPost]
    public async Task<ActionResult<StringTypeDto>> CreateStringType(CreateStringTypeDto createStringTypeDto)
    {
        var stringType = new StringType
        {
            Brand = createStringTypeDto.Brand,
            Model = createStringTypeDto.Model,
            Gauge = createStringTypeDto.Gauge,
            Material = createStringTypeDto.Material,
            Color = createStringTypeDto.Color
        };

        _context.StringTypes.Add(stringType);
        await _context.SaveChangesAsync();

        var stringTypeDto = new StringTypeDto
        {
            Id = stringType.Id,
            Brand = stringType.Brand,
            Model = stringType.Model,
            Gauge = stringType.Gauge,
            Material = stringType.Material,
            Color = stringType.Color
        };

        return CreatedAtAction(nameof(GetStringType), new { id = stringType.Id }, stringTypeDto);
    }

    // PUT: api/StringTypes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStringType(int id, UpdateStringTypeDto updateStringTypeDto)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
        {
            return NotFound();
        }

        stringType.Brand = updateStringTypeDto.Brand;
        stringType.Model = updateStringTypeDto.Model;
        stringType.Gauge = updateStringTypeDto.Gauge;
        stringType.Material = updateStringTypeDto.Material;
        stringType.Color = updateStringTypeDto.Color;

        _context.Entry(stringType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringTypeExists(id))
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

    // DELETE: api/StringTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStringType(int id)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
        {
            return NotFound();
        }

        // Verificar si el tipo de cuerda se está utilizando en algún trabajo de encordado
        var isUsedAsMain = await _context.StringJobs.AnyAsync(sj => sj.MainStringId == id);
        var isUsedAsCross = await _context.StringJobs.AnyAsync(sj => sj.CrossStringId == id);

        if (isUsedAsMain || isUsedAsCross)
        {
            return BadRequest("No se puede eliminar el tipo de cuerda porque está siendo utilizado en trabajos de encordado.");
        }

        _context.StringTypes.Remove(stringType);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StringTypeExists(int id)
    {
        return _context.StringTypes.Any(e => e.Id == id);
    }
}