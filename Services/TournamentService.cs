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
        var tournaments = await _context.Tournaments.ToListAsync();

        return tournaments.Select(t => new TournamentDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Location = t.Location,
            Category = t.Category
        });
    }

    public async Task<TournamentDto?> GetByIdAsync(int id)
    {
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
            return null;

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

    public async Task<TournamentDto?> GetCurrentTournamentAsync()
    {
        var currentDate = DateTime.Now.Date;

        var tournament = await _context.Tournaments
            .Where(t => t.StartDate <= currentDate && t.EndDate >= currentDate)
            .OrderBy(t => t.StartDate)
            .FirstOrDefaultAsync();

        if (tournament == null)
            return null;

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
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null)
            return false;

        // Verificar si el torneo está asociado a algún trabajo de encordado
        var hasStringJobs = await _context.StringJobs.AnyAsync(sj => sj.TournamentId == id);

        if (hasStringJobs)
            return false;

        _context.Tournaments.Remove(tournament);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<bool> TournamentExistsAsync(int id)
    {
        return await _context.Tournaments.AnyAsync(e => e.Id == id);
    }
}