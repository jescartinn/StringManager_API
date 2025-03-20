using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class StringTypeService : IStringTypeService
{
    private readonly ApplicationDbContext _context;

    public StringTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StringTypeDto>> GetAllAsync()
    {
        var stringTypes = await _context.StringTypes.ToListAsync();

        return stringTypes.Select(st => new StringTypeDto
        {
            Id = st.Id,
            Brand = st.Brand,
            Model = st.Model,
            Gauge = st.Gauge,
            Material = st.Material,
            Color = st.Color
        });
    }

    public async Task<StringTypeDto?> GetByIdAsync(int id)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
            return null;

        return new StringTypeDto
        {
            Id = stringType.Id,
            Brand = stringType.Brand,
            Model = stringType.Model,
            Gauge = stringType.Gauge,
            Material = stringType.Material,
            Color = stringType.Color
        };
    }

    public async Task<StringTypeDto> CreateAsync(CreateStringTypeDto createDto)
    {
        var stringType = new StringType
        {
            Brand = createDto.Brand,
            Model = createDto.Model,
            Gauge = createDto.Gauge,
            Material = createDto.Material,
            Color = createDto.Color
        };

        _context.StringTypes.Add(stringType);
        await _context.SaveChangesAsync();

        return new StringTypeDto
        {
            Id = stringType.Id,
            Brand = stringType.Brand,
            Model = stringType.Model,
            Gauge = stringType.Gauge,
            Material = stringType.Material,
            Color = stringType.Color
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateStringTypeDto updateDto)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
            return false;

        stringType.Brand = updateDto.Brand;
        stringType.Model = updateDto.Model;
        stringType.Gauge = updateDto.Gauge;
        stringType.Material = updateDto.Material;
        stringType.Color = updateDto.Color;

        _context.Entry(stringType).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringTypeExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var stringType = await _context.StringTypes.FindAsync(id);

        if (stringType == null)
            return false;

        // Verificar si el tipo de cuerda se está utilizando en algún trabajo de encordado
        var isUsedAsMain = await _context.StringJobs.AnyAsync(sj => sj.MainStringId == id);
        var isUsedAsCross = await _context.StringJobs.AnyAsync(sj => sj.CrossStringId == id);

        if (isUsedAsMain || isUsedAsCross)
            return false;

        _context.StringTypes.Remove(stringType);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> StringTypeExistsAsync(int id)
    {
        return await _context.StringTypes.AnyAsync(e => e.Id == id);
    }
}