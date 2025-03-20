using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StringersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StringersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Stringers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StringerDto>>> GetStringers()
    {
        var stringers = await _context.Stringers.ToListAsync();

        var stringerDtos = stringers.Select(s => new StringerDto
        {
            Id = s.Id,
            Name = s.Name,
            LastName = s.LastName,
            Email = s.Email,
            PhoneNumber = s.PhoneNumber
        }).ToList();

        return Ok(stringerDtos);
    }

    // GET: api/Stringers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StringerDto>> GetStringer(int id)
    {
        var stringer = await _context.Stringers.FindAsync(id);

        if (stringer == null)
        {
            return NotFound();
        }

        var stringerDto = new StringerDto
        {
            Id = stringer.Id,
            Name = stringer.Name,
            LastName = stringer.LastName,
            Email = stringer.Email,
            PhoneNumber = stringer.PhoneNumber
        };

        return Ok(stringerDto);
    }

    // POST: api/Stringers
    [HttpPost]
    public async Task<ActionResult<StringerDto>> CreateStringer(CreateStringerDto createStringerDto)
    {
        var stringer = new Stringer
        {
            Name = createStringerDto.Name,
            LastName = createStringerDto.LastName,
            Email = createStringerDto.Email,
            PhoneNumber = createStringerDto.PhoneNumber
        };

        _context.Stringers.Add(stringer);
        await _context.SaveChangesAsync();

        var stringerDto = new StringerDto
        {
            Id = stringer.Id,
            Name = stringer.Name,
            LastName = stringer.LastName,
            Email = stringer.Email,
            PhoneNumber = stringer.PhoneNumber
        };

        return CreatedAtAction(nameof(GetStringer), new { id = stringer.Id }, stringerDto);
    }

    // PUT: api/Stringers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStringer(int id, UpdateStringerDto updateStringerDto)
    {
        var stringer = await _context.Stringers.FindAsync(id);

        if (stringer == null)
        {
            return NotFound();
        }

        stringer.Name = updateStringerDto.Name;
        stringer.LastName = updateStringerDto.LastName;
        stringer.Email = updateStringerDto.Email;
        stringer.PhoneNumber = updateStringerDto.PhoneNumber;

        _context.Entry(stringer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StringerExists(id))
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

    // DELETE: api/Stringers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStringer(int id)
    {
        var stringer = await _context.Stringers.FindAsync(id);

        if (stringer == null)
        {
            return NotFound();
        }

        // Verificar si el encordador está asignado a algún trabajo de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.StringerId == id);

        if (hasStringJobs)
        {
            return BadRequest("No se puede eliminar el encordador porque está asignado a trabajos de encordado.");
        }

        _context.Stringers.Remove(stringer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StringerExists(int id)
    {
        return _context.Stringers.Any(e => e.Id == id);
    }
}