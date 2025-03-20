using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public class StringJobService : IStringJobService
{
    private readonly ApplicationDbContext _context;

    public StringJobService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StringJobDto>> GetAllAsync()
    {
        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        return stringJobs.Select(sj => MapToStringJobDto(sj));
    }

    public async Task<StringJobDto?> GetByIdAsync(int id)
    {
        var stringJob = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .FirstOrDefaultAsync(sj => sj.Id == id);

        if (stringJob == null)
            return null;

        return MapToStringJobDto(stringJob);
    }

    public async Task<IEnumerable<StringJobDto>> GetByStatusAsync(string status)
    {
        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.Status == status)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        return stringJobs.Select(sj => MapToStringJobDto(sj));
    }

    public async Task<IEnumerable<StringJobDto>> GetByTournamentIdAsync(int tournamentId)
    {
        // Verificar si el torneo existe
        var tournamentExists = await _context.Tournaments.AnyAsync(t => t.Id == tournamentId);
        if (!tournamentExists)
            return Enumerable.Empty<StringJobDto>();

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.TournamentId == tournamentId)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        return stringJobs.Select(sj => MapToStringJobDto(sj));
    }

    public async Task<IEnumerable<StringJobDto>> GetByPlayerIdAsync(int playerId)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == playerId);
        if (!playerExists)
            return Enumerable.Empty<StringJobDto>();

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.PlayerId == playerId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        return stringJobs.Select(sj => MapToStringJobDto(sj));
    }

    public async Task<IEnumerable<StringJobDto>> GetByStringerIdAsync(int stringerId)
    {
        // Verificar si el encordador existe
        var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == stringerId);
        if (!stringerExists)
            return Enumerable.Empty<StringJobDto>();

        var stringJobs = await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Where(sj => sj.StringerId == stringerId)
            .OrderByDescending(sj => sj.CreatedAt)
            .ToListAsync();

        return stringJobs.Select(sj => MapToStringJobDto(sj));
    }

    public async Task<StringJobDto> CreateAsync(CreateStringJobDto createDto)
    {
        // Verificaciones
        var playerExists = await _context.Players.AnyAsync(p => p.Id == createDto.PlayerId);
        if (!playerExists)
            throw new InvalidOperationException("El jugador especificado no existe.");

        var racquet = await _context.Racquets.FindAsync(createDto.RacquetId);
        if (racquet == null)
            throw new InvalidOperationException("La raqueta especificada no existe.");
            
        if (racquet.PlayerId != createDto.PlayerId)
            throw new InvalidOperationException("La raqueta especificada no pertenece al jugador indicado.");

        if (createDto.MainStringId.HasValue)
        {
            var mainStringExists = await _context.StringTypes.AnyAsync(st => st.Id == createDto.MainStringId);
            if (!mainStringExists)
                throw new InvalidOperationException("La cuerda principal especificada no existe.");
        }

        if (createDto.CrossStringId.HasValue)
        {
            var crossStringExists = await _context.StringTypes.AnyAsync(st => st.Id == createDto.CrossStringId);
            if (!crossStringExists)
                throw new InvalidOperationException("La cuerda cruzada especificada no existe.");
        }

        if (createDto.StringerId.HasValue)
        {
            var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == createDto.StringerId);
            if (!stringerExists)
                throw new InvalidOperationException("El encordador especificado no existe.");
        }

        if (createDto.TournamentId.HasValue)
        {
            var tournamentExists = await _context.Tournaments.AnyAsync(t => t.Id == createDto.TournamentId);
            if (!tournamentExists)
                throw new InvalidOperationException("El torneo especificado no existe.");
        }

        var stringJob = new StringJob
        {
            PlayerId = createDto.PlayerId,
            RacquetId = createDto.RacquetId,
            MainStringId = createDto.MainStringId,
            CrossStringId = createDto.CrossStringId,
            StringerId = createDto.StringerId,
            TournamentId = createDto.TournamentId,
            MainTension = createDto.MainTension,
            CrossTension = createDto.CrossTension,
            IsTensionInKg = createDto.IsTensionInKg,
            Notes = createDto.Notes,
            Priority = createDto.Priority,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _context.StringJobs.Add(stringJob);
        await _context.SaveChangesAsync();

        // Cargar entidades relacionadas
        await LoadStringJobRelatedEntitiesAsync(stringJob);

        return MapToStringJobDto(stringJob);
    }

    public async Task<bool> UpdateAsync(int id, UpdateStringJobDto updateDto)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);
        
        if (stringJob == null)
            return false;

        // Verificar si el trabajo ya está completado o cancelado
        if (stringJob.Status == "Completed" || stringJob.Status == "Cancelled")
            return false;

        // Verificar las cuerdas si se proporcionan
        if (updateDto.MainStringId.HasValue)
        {
            var mainStringExists = await _context.StringTypes.AnyAsync(st => st.Id == updateDto.MainStringId);
            if (!mainStringExists)
                return false;
                
            stringJob.MainStringId = updateDto.MainStringId;
        }

        if (updateDto.CrossStringId.HasValue)
        {
            var crossStringExists = await _context.StringTypes.AnyAsync(st => st.Id == updateDto.CrossStringId);
            if (!crossStringExists)
                return false;
                
            stringJob.CrossStringId = updateDto.CrossStringId;
        }

        // Verificar el encordador si se proporciona
        if (updateDto.StringerId.HasValue)
        {
            var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == updateDto.StringerId);
            if (!stringerExists)
                return false;
                
            stringJob.StringerId = updateDto.StringerId;
        }

        stringJob.MainTension = updateDto.MainTension;
        stringJob.CrossTension = updateDto.CrossTension;
        stringJob.IsTensionInKg = updateDto.IsTensionInKg;
        stringJob.Status = updateDto.Status;
        stringJob.Notes = updateDto.Notes;
        stringJob.Priority = updateDto.Priority;

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringJobExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);
        
        if (stringJob == null)
            return false;

        // Solo permitir eliminar trabajos en estado Pending
        if (stringJob.Status != "Pending")
            return false;

        _context.StringJobs.Remove(stringJob);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CompleteJobAsync(int id, CompleteStringJobDto completeDto)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);
        
        if (stringJob == null)
            return false;

        // Verificar si el trabajo ya está completado o cancelado
        if (stringJob.Status == "Completed")
            return false;
            
        if (stringJob.Status == "Cancelled")
            return false;

        stringJob.Status = "Completed";
        stringJob.CompletedAt = completeDto.CompletedAt;
        
        if (!string.IsNullOrEmpty(completeDto.Notes))
        {
            stringJob.Notes = (string.IsNullOrEmpty(stringJob.Notes))
                ? completeDto.Notes
                : $"{stringJob.Notes}\n\nNotas de finalización: {completeDto.Notes}";
        }

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringJobExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> CancelJobAsync(int id, string? cancelReason)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);
        
        if (stringJob == null)
            return false;

        // Verificar si el trabajo ya está completado o cancelado
        if (stringJob.Status == "Completed")
            return false;
            
        if (stringJob.Status == "Cancelled")
            return false;

        stringJob.Status = "Cancelled";
        
        if (!string.IsNullOrEmpty(cancelReason))
        {
            stringJob.Notes = (string.IsNullOrEmpty(stringJob.Notes))
                ? $"Cancelado: {cancelReason}"
                : $"{stringJob.Notes}\n\nCancelado: {cancelReason}";
        }

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringJobExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    public async Task<bool> StartJobAsync(int id)
    {
        var stringJob = await _context.StringJobs.FindAsync(id);
        
        if (stringJob == null)
            return false;

        // Solo se pueden iniciar trabajos pendientes
        if (stringJob.Status != "Pending")
            return false;

        stringJob.Status = "InProgress";

        _context.Entry(stringJob).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StringJobExistsAsync(id))
                return false;
            else
                throw;
        }
    }

    private async Task<bool> StringJobExistsAsync(int id)
    {
        return await _context.StringJobs.AnyAsync(e => e.Id == id);
    }

    private async Task LoadStringJobRelatedEntitiesAsync(StringJob stringJob)
    {
        await _context.Entry(stringJob)
            .Reference(sj => sj.Player)
            .LoadAsync();
            
        await _context.Entry(stringJob)
            .Reference(sj => sj.Racquet)
            .LoadAsync();
            
        if (stringJob.MainStringId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.MainString)
                .LoadAsync();
        }
        
        if (stringJob.CrossStringId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.CrossString)
                .LoadAsync();
        }
        
        if (stringJob.StringerId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.Stringer)
                .LoadAsync();
        }
        
        if (stringJob.TournamentId.HasValue)
        {
            await _context.Entry(stringJob)
                .Reference(sj => sj.Tournament)
                .LoadAsync();
        }
    }

    private StringJobDto MapToStringJobDto(StringJob stringJob)
    {
        return new StringJobDto
        {
            Id = stringJob.Id,
            PlayerId = stringJob.PlayerId,
            Player = stringJob.Player != null ? new PlayerDto
            {
                Id = stringJob.Player.Id,
                Name = stringJob.Player.Name,
                LastName = stringJob.Player.LastName,
                CountryCode = stringJob.Player.CountryCode
            } : null,
            RacquetId = stringJob.RacquetId,
            Racquet = stringJob.Racquet != null ? new RacquetDto
            {
                Id = stringJob.Racquet.Id,
                PlayerId = stringJob.Racquet.PlayerId,
                Brand = stringJob.Racquet.Brand,
                Model = stringJob.Racquet.Model,
                SerialNumber = stringJob.Racquet.SerialNumber,
                HeadSize = stringJob.Racquet.HeadSize,
                Notes = stringJob.Racquet.Notes
            } : null,
            MainStringId = stringJob.MainStringId,
            MainString = stringJob.MainString != null ? new StringTypeDto
            {
                Id = stringJob.MainString.Id,
                Brand = stringJob.MainString.Brand,
                Model = stringJob.MainString.Model,
                Gauge = stringJob.MainString.Gauge,
                Material = stringJob.MainString.Material,
                Color = stringJob.MainString.Color
            } : null,
            CrossStringId = stringJob.CrossStringId,
            CrossString = stringJob.CrossString != null ? new StringTypeDto
            {
                Id = stringJob.CrossString.Id,
                Brand = stringJob.CrossString.Brand,
                Model = stringJob.CrossString.Model,
                Gauge = stringJob.CrossString.Gauge,
                Material = stringJob.CrossString.Material,
                Color = stringJob.CrossString.Color
            } : null,
            StringerId = stringJob.StringerId,
            Stringer = stringJob.Stringer != null ? new StringerDto
            {
                Id = stringJob.Stringer.Id,
                Name = stringJob.Stringer.Name,
                LastName = stringJob.Stringer.LastName,
                Email = stringJob.Stringer.Email,
                PhoneNumber = stringJob.Stringer.PhoneNumber
            } : null,
            TournamentId = stringJob.TournamentId,
            Tournament = stringJob.Tournament != null ? new TournamentDto
            {
                Id = stringJob.Tournament.Id,
                Name = stringJob.Tournament.Name,
                StartDate = stringJob.Tournament.StartDate,
                EndDate = stringJob.Tournament.EndDate,
                Location = stringJob.Tournament.Location,
                Category = stringJob.Tournament.Category
            } : null,
            CreatedAt = stringJob.CreatedAt,
            CompletedAt = stringJob.CompletedAt,
            MainTension = stringJob.MainTension,
            CrossTension = stringJob.CrossTension,
            IsTensionInKg = stringJob.IsTensionInKg,
            Status = stringJob.Status,
            Notes = stringJob.Notes,
            Priority = stringJob.Priority
        };
    }
}