using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class RacquetService : IRacquetService
{
    private readonly ApplicationDbContext _context;

    public RacquetService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RacquetDto>> GetAllAsync()
    {
        return await _context.Racquets
            .Include(r => r.Player)
            .Select(r => new RacquetDto
            {
                Id = r.Id,
                PlayerId = r.PlayerId,
                Brand = r.Brand,
                Model = r.Model,
                SerialNumber = r.SerialNumber,
                HeadSize = r.HeadSize,
                Notes = r.Notes,
                Player = r.Player != null ? new PlayerDto
                {
                    Id = r.Player.Id,
                    Name = r.Player.Name,
                    LastName = r.Player.LastName,
                    CountryCode = r.Player.CountryCode
                } : null
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<RacquetDto>> GetByPlayerIdAsync(int playerId)
    {
        return await _context.Racquets
            .Include(r => r.Player)
            .Where(r => r.PlayerId == playerId)
            .Select(r => new RacquetDto
            {
                Id = r.Id,
                PlayerId = r.PlayerId,
                Brand = r.Brand,
                Model = r.Model,
                SerialNumber = r.SerialNumber,
                HeadSize = r.HeadSize,
                Notes = r.Notes,
                Player = r.Player != null ? new PlayerDto
                {
                    Id = r.Player.Id,
                    Name = r.Player.Name,
                    LastName = r.Player.LastName,
                    CountryCode = r.Player.CountryCode
                } : null
            })
            .ToListAsync();
    }

    public async Task<RacquetDto?> GetByIdAsync(int id)
    {
        return await _context.Racquets
            .Include(r => r.Player)
            .Where(r => r.Id == id)
            .Select(r => new RacquetDto
            {
                Id = r.Id,
                PlayerId = r.PlayerId,
                Brand = r.Brand,
                Model = r.Model,
                SerialNumber = r.SerialNumber,
                HeadSize = r.HeadSize,
                Notes = r.Notes,
                Player = r.Player != null ? new PlayerDto
                {
                    Id = r.Player.Id,
                    Name = r.Player.Name,
                    LastName = r.Player.LastName,
                    CountryCode = r.Player.CountryCode
                } : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<RacquetDto> CreateAsync(CreateRacquetDto createDto)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == createDto.PlayerId);
        if (!playerExists)
            throw new InvalidOperationException("El jugador especificado no existe.");

        var racquet = new Racquet
        {
            PlayerId = createDto.PlayerId,
            Brand = createDto.Brand,
            Model = createDto.Model,
            SerialNumber = createDto.SerialNumber,
            HeadSize = createDto.HeadSize,
            Notes = createDto.Notes
        };

        _context.Racquets.Add(racquet);
        await _context.SaveChangesAsync();

        // Obtener los datos completos del jugador
        var playerInfo = await _context.Players
            .Where(p => p.Id == racquet.PlayerId)
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                LastName = p.LastName,
                CountryCode = p.CountryCode
            })
            .FirstOrDefaultAsync();

        return new RacquetDto
        {
            Id = racquet.Id,
            PlayerId = racquet.PlayerId,
            Brand = racquet.Brand,
            Model = racquet.Model,
            SerialNumber = racquet.SerialNumber,
            HeadSize = racquet.HeadSize,
            Notes = racquet.Notes,
            Player = playerInfo
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateRacquetDto updateDto)
    {
        var racquet = await _context.Racquets.FindAsync(id);

        if (racquet == null)
            return false;

        racquet.Brand = updateDto.Brand;
        racquet.Model = updateDto.Model;
        racquet.SerialNumber = updateDto.SerialNumber;
        racquet.HeadSize = updateDto.HeadSize;
        racquet.Notes = updateDto.Notes;

        _context.Entry(racquet).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await RacquetExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var racquet = await _context.Racquets.FindAsync(id);

        if (racquet == null)
            return false;

        // Verificar si la raqueta tiene trabajos de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.RacquetId == id);

        if (hasStringJobs)
            return false;

        _context.Racquets.Remove(racquet);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> RacquetExistsAsync(int id)
    {
        return await _context.Racquets.AnyAsync(e => e.Id == id);
    }
}