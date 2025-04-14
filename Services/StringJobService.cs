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
        return await _context.StringJobs
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .ToListAsync();
    }

    public async Task<StringJobDto?> GetByIdAsync(int id)
    {
        return await _context.StringJobs
            .Where(sj => sj.Id == id)
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<StringJobDto>> GetByStatusAsync(string status)
    {
        return await _context.StringJobs
            .Where(sj => sj.Status == status)
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<StringJobDto>> GetByTournamentIdAsync(int tournamentId)
    {
        // Verificar si el torneo existe
        var tournamentExists = await _context.Tournaments.AnyAsync(t => t.Id == tournamentId);
        if (!tournamentExists)
            return Enumerable.Empty<StringJobDto>();

        return await _context.StringJobs
            .Where(sj => sj.TournamentId == tournamentId)
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.Priority)
            .ThenByDescending(sj => sj.CreatedAt)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<StringJobDto>> GetByPlayerIdAsync(int playerId)
    {
        // Verificar si el jugador existe
        var playerExists = await _context.Players.AnyAsync(p => p.Id == playerId);
        if (!playerExists)
            return Enumerable.Empty<StringJobDto>();

        return await _context.StringJobs
            .Where(sj => sj.PlayerId == playerId)
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.CreatedAt)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<StringJobDto>> GetByStringerIdAsync(int stringerId)
    {
        // Verificar si el encordador existe
        var stringerExists = await _context.Stringers.AnyAsync(s => s.Id == stringerId);
        if (!stringerExists)
            return Enumerable.Empty<StringJobDto>();

        return await _context.StringJobs
            .Where(sj => sj.StringerId == stringerId)
            .Include(sj => sj.Player)
            .Include(sj => sj.Racquet)
            .Include(sj => sj.MainString)
            .Include(sj => sj.CrossString)
            .Include(sj => sj.Stringer)
            .Include(sj => sj.Tournament)
            .OrderByDescending(sj => sj.CreatedAt)
            .Select(sj => new StringJobDto
            {
                Id = sj.Id,
                PlayerId = sj.PlayerId,
                Player = sj.Player != null ? new PlayerDto
                {
                    Id = sj.Player.Id,
                    Name = sj.Player.Name,
                    LastName = sj.Player.LastName,
                    CountryCode = sj.Player.CountryCode
                } : null,
                RacquetId = sj.RacquetId,
                Racquet = sj.Racquet != null ? new RacquetDto
                {
                    Id = sj.Racquet.Id,
                    PlayerId = sj.Racquet.PlayerId,
                    Brand = sj.Racquet.Brand,
                    Model = sj.Racquet.Model,
                    SerialNumber = sj.Racquet.SerialNumber,
                    HeadSize = sj.Racquet.HeadSize,
                    Notes = sj.Racquet.Notes
                } : null,
                MainStringId = sj.MainStringId,
                MainString = sj.MainString != null ? new StringTypeDto
                {
                    Id = sj.MainString.Id,
                    Brand = sj.MainString.Brand,
                    Model = sj.MainString.Model,
                    Gauge = sj.MainString.Gauge,
                    Material = sj.MainString.Material,
                    Color = sj.MainString.Color
                } : null,
                CrossStringId = sj.CrossStringId,
                CrossString = sj.CrossString != null ? new StringTypeDto
                {
                    Id = sj.CrossString.Id,
                    Brand = sj.CrossString.Brand,
                    Model = sj.CrossString.Model,
                    Gauge = sj.CrossString.Gauge,
                    Material = sj.CrossString.Material,
                    Color = sj.CrossString.Color
                } : null,
                StringerId = sj.StringerId,
                Stringer = sj.Stringer != null ? new StringerDto
                {
                    Id = sj.Stringer.Id,
                    Name = sj.Stringer.Name,
                    LastName = sj.Stringer.LastName,
                    Email = sj.Stringer.Email,
                    PhoneNumber = sj.Stringer.PhoneNumber
                } : null,
                TournamentId = sj.TournamentId,
                Tournament = sj.Tournament != null ? new TournamentDto
                {
                    Id = sj.Tournament.Id,
                    Name = sj.Tournament.Name,
                    StartDate = sj.Tournament.StartDate,
                    EndDate = sj.Tournament.EndDate,
                    Location = sj.Tournament.Location,
                    Category = sj.Tournament.Category
                } : null,
                CreatedAt = sj.CreatedAt,
                CompletedAt = sj.CompletedAt,
                MainTension = sj.MainTension,
                CrossTension = sj.CrossTension,
                IsTensionInKg = sj.IsTensionInKg,
                Status = sj.Status,
                Notes = sj.Notes,
                Priority = sj.Priority
            })
            .ToListAsync();
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
            Logo = createDto.Logo,
            Notes = createDto.Notes,
            Priority = createDto.Priority,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _context.StringJobs.Add(stringJob);
        await _context.SaveChangesAsync();

        var result = await GetByIdAsync(stringJob.Id);
        if (result == null)
            throw new InvalidOperationException("No se pudo recuperar el trabajo de encordado creado.");
        return result;
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
        stringJob.Logo = updateDto.Logo;
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
}