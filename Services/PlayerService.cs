using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class PlayerService : IPlayerService
{
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlayerDto>> GetAllAsync()
    {
        return await _context.Players
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                LastName = p.LastName,
                CountryCode = p.CountryCode
            })
            .ToListAsync();
    }

    public async Task<PlayerDto?> GetByIdAsync(int id)
    {
        return await _context.Players
            .Where(p => p.Id == id)
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                LastName = p.LastName,
                CountryCode = p.CountryCode
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PlayerDto> CreateAsync(CreatePlayerDto createDto)
    {
        var player = new Player
        {
            Name = createDto.Name,
            LastName = createDto.LastName,
            CountryCode = createDto.CountryCode
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            LastName = player.LastName,
            CountryCode = player.CountryCode
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdatePlayerDto updateDto)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
            return false;

        player.Name = updateDto.Name;
        player.LastName = updateDto.LastName;
        player.CountryCode = updateDto.CountryCode;

        _context.Entry(player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PlayerExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Verificar la existencia del jugador y sus dependencias
        var playerWithDependencies = await _context.Players
            .Where(p => p.Id == id)
            .Select(p => new
            {
                Player = p,
                HasStringJobs = p.StringJobs.Any()
            })
            .FirstOrDefaultAsync();

        if (playerWithDependencies == null)
            return false;

        // Verificar si el jugador tiene trabajos de encordado
        if (playerWithDependencies.HasStringJobs)
            return false;

        _context.Players.Remove(playerWithDependencies.Player);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> PlayerExistsAsync(int id)
    {
        return await _context.Players.AnyAsync(e => e.Id == id);
    }
}