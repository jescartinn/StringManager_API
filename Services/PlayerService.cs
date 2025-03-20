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
        var players = await _context.Players.ToListAsync();
        
        return players.Select(p => new PlayerDto
        {
            Id = p.Id,
            Name = p.Name,
            LastName = p.LastName,
            CountryCode = p.CountryCode
        });
    }

    public async Task<PlayerDto?> GetByIdAsync(int id)
    {
        var player = await _context.Players.FindAsync(id);
        
        if (player == null)
            return null;
            
        return new PlayerDto
        {
            Id = player.Id,
            Name = player.Name,
            LastName = player.LastName,
            CountryCode = player.CountryCode
        };
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
        var player = await _context.Players.FindAsync(id);
        
        if (player == null)
            return false;

        // Verificar si el jugador tiene trabajos de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.PlayerId == id);
        
        if (hasStringJobs)
            return false;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> PlayerExistsAsync(int id)
    {
        return await _context.Players.AnyAsync(e => e.Id == id);
    }
}