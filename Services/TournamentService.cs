using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class TournamentService : ITournamentService
{
    private readonly ApplicationDbContext _context;

    public TournamentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllAsync()
    {
        return await _context.Tournaments
            .Select(t => new TournamentDto
            {
                Id = t.Id,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Location = t.Location,
                Category = t.Category
            })
            .ToListAsync();
    }

    public async Task<TournamentDto?> GetByIdAsync(int id)
    {
        return await _context.Tournaments
            .Where(t => t.Id == id)
            .Select(t => new TournamentDto
            {
                Id = t.Id,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Location = t.Location,
                Category = t.Category
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TournamentDto?> GetCurrentTournamentAsync()
    {
        var currentDate = DateTime.Now.Date;

        return await _context.Tournaments
            .Where(t => t.StartDate <= currentDate && t.EndDate >= currentDate)
            .OrderBy(t => t.StartDate)
            .Select(t => new TournamentDto
            {
                Id = t.Id,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Location = t.Location,
                Category = t.Category
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TournamentDto> CreateAsync(CreateTournamentDto createDto)
    {
        // Validar fechas
        if (createDto.EndDate < createDto.StartDate)
            throw new InvalidOperationException("La fecha de finalización no puede ser anterior a la fecha de inicio.");

        var tournament = new Tournament
        {
            Name = createDto.Name,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            Location = createDto.Location,
            Category = createDto.Category
        };

        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        return new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Category = tournament.Category
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTournamentDto updateDto)
    {
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
            return false;

        // Validar fechas
        if (updateDto.EndDate < updateDto.StartDate)
            throw new InvalidOperationException("La fecha de finalización no puede ser anterior a la fecha de inicio.");

        tournament.Name = updateDto.Name;
        tournament.StartDate = updateDto.StartDate;
        tournament.EndDate = updateDto.EndDate;
        tournament.Location = updateDto.Location;
        tournament.Category = updateDto.Category;

        _context.Entry(tournament).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TournamentExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Verificar la existencia y dependencias
        var tournamentWithDependencies = await _context.Tournaments
            .Where(t => t.Id == id)
            .Select(t => new
            {
                Tournament = t,
                HasStringJobs = _context.StringJobs.Any(sj => sj.TournamentId == id)
            })
            .FirstOrDefaultAsync();

        if (tournamentWithDependencies == null)
            return false;

        // Verificar si el torneo está asociado a algún trabajo de encordado
        if (tournamentWithDependencies.HasStringJobs)
            return false;

        _context.Tournaments.Remove(tournamentWithDependencies.Tournament);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> TournamentExistsAsync(int id)
    {
        return await _context.Tournaments.AnyAsync(e => e.Id == id);
    }
}