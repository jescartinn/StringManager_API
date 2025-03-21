using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class StringerService : IStringerService
{
    private readonly ApplicationDbContext _context;

    public StringerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StringerDto>> GetAllAsync()
    {
        return await _context.Stringers
            .Select(s => new StringerDto
            {
                Id = s.Id,
                Name = s.Name,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            })
            .ToListAsync();
    }

    public async Task<StringerDto?> GetByIdAsync(int id)
    {
        return await _context.Stringers
            .Where(s => s.Id == id)
            .Select(s => new StringerDto
            {
                Id = s.Id,
                Name = s.Name,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StringerDto> CreateAsync(CreateStringerDto createDto)
    {
        var stringer = new Stringer
        {
            Name = createDto.Name,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PhoneNumber = createDto.PhoneNumber
        };

        _context.Stringers.Add(stringer);
        await _context.SaveChangesAsync();

        return new StringerDto
        {
            Id = stringer.Id,
            Name = stringer.Name,
            LastName = stringer.LastName,
            Email = stringer.Email,
            PhoneNumber = stringer.PhoneNumber
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateStringerDto updateDto)
    {
        var stringer = await _context.Stringers.FindAsync(id);

        if (stringer == null)
            return false;

        stringer.Name = updateDto.Name;
        stringer.LastName = updateDto.LastName;
        stringer.Email = updateDto.Email;
        stringer.PhoneNumber = updateDto.PhoneNumber;

        _context.Entry(stringer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringerExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Verificar la existencia y dependencias
        var stringerWithDependencies = await _context.Stringers
            .Where(s => s.Id == id)
            .Select(s => new
            {
                Stringer = s,
                HasStringJobs = _context.StringJobs.Any(sj => sj.StringerId == id)
            })
            .FirstOrDefaultAsync();

        if (stringerWithDependencies == null)
            return false;

        // Verificar si el encordador está asignado a algún trabajo de encordado
        if (stringerWithDependencies.HasStringJobs)
            return false;

        _context.Stringers.Remove(stringerWithDependencies.Stringer);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> StringerExistsAsync(int id)
    {
        return await _context.Stringers.AnyAsync(e => e.Id == id);
    }
}