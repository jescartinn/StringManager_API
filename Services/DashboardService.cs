using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;

namespace StringManager_API.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetDashboardStatsAsync()
    {
        var today = DateTime.Now.Date;

        // Obtener torneo actual
        var currentTournament = await _context.Tournaments
            .Where(t => t.StartDate <= today && t.EndDate >= today)
            .OrderBy(t => t.StartDate)
            .Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                EndDate = t.EndDate
            })
            .FirstOrDefaultAsync();

        // Estadísticas de trabajos
        var jobStats = await _context.StringJobs
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalPendingJobs = g.Count(sj => sj.Status == "Pending"),
                TotalInProgressJobs = g.Count(sj => sj.Status == "InProgress"),
                TotalCompletedJobsToday = g.Count(sj => sj.Status == "Completed" &&
                                                sj.CompletedAt.HasValue &&
                                                sj.CompletedAt.Value.Date == today),
                HighPriorityJobs = g.Count(sj => (sj.Status == "Pending" || sj.Status == "InProgress") &&
                                         sj.Priority == 1)
            })
            .FirstOrDefaultAsync() ?? new
            {
                TotalPendingJobs = 0,
                TotalInProgressJobs = 0,
                TotalCompletedJobsToday = 0,
                HighPriorityJobs = 0
            };

        // Mejor encordadores usando
        var topStringers = await _context.StringJobs
            .Where(sj => sj.Status == "Completed" &&
                    sj.CompletedAt.HasValue &&
                    sj.StringerId.HasValue)
            .Join(
                _context.Stringers,
                sj => sj.StringerId,
                s => s.Id,
                (sj, s) => new { StringJob = sj, Stringer = s }
            )
            .GroupBy(x => new
            {
                StringerId = x.Stringer.Id,
                StringerName = x.Stringer.Name + " " + x.Stringer.LastName
            })
            .Select(g => new
            {
                StringerId = g.Key.StringerId,
                StringerName = g.Key.StringerName,
                CompletedJobs = g.Count()
            })
            .OrderByDescending(x => x.CompletedJobs)
            .Take(5)
            .ToListAsync();

        // Mejores jugadores
        var topPlayers = await _context.StringJobs
            .Join(
                _context.Players,
                sj => sj.PlayerId,
                p => p.Id,
                (sj, p) => new { StringJob = sj, Player = p }
            )
            .GroupBy(x => new
            {
                PlayerId = x.Player.Id,
                PlayerName = x.Player.Name + " " + x.Player.LastName
            })
            .Select(g => new
            {
                PlayerId = g.Key.PlayerId,
                PlayerName = g.Key.PlayerName,
                TotalJobs = g.Count()
            })
            .OrderByDescending(x => x.TotalJobs)
            .Take(5)
            .ToListAsync();

        // Mejores cuerdas
        var topStrings = await _context.StringJobs
            .Where(sj => sj.MainStringId.HasValue)
            .Join(
                _context.StringTypes,
                sj => sj.MainStringId,
                st => st.Id,
                (sj, st) => new { StringJob = sj, StringType = st }
            )
            .GroupBy(x => new
            {
                StringId = x.StringType.Id,
                StringName = x.StringType.Brand + " " + x.StringType.Model
            })
            .Select(g => new
            {
                StringId = g.Key.StringId,
                StringName = g.Key.StringName,
                TotalUses = g.Count()
            })
            .OrderByDescending(x => x.TotalUses)
            .Take(5)
            .ToListAsync();

        return new
        {
            CurrentTournament = currentTournament != null ? new
            {
                Id = currentTournament.Id,
                Name = currentTournament.Name,
                RemainingDays = (currentTournament.EndDate - today).Days
            } : null,
            PendingJobs = jobStats.TotalPendingJobs,
            InProgressJobs = jobStats.TotalInProgressJobs,
            CompletedJobsToday = jobStats.TotalCompletedJobsToday,
            HighPriorityJobs = jobStats.HighPriorityJobs,
            TopStringers = topStringers,
            TopPlayers = topPlayers,
            TopStrings = topStrings
        };
    }

    public async Task<object> GetDistributionStatsAsync(int? tournamentId = null)
    {
        IQueryable<Models.StringJob> query = _context.StringJobs;

        if (tournamentId.HasValue)
        {
            query = query.Where(sj => sj.TournamentId == tournamentId);
        }

        // Distribución por estado
        var statusDistribution = await query
            .GroupBy(sj => sj.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        // Distribución por tensión (en rangos)
        var tensionDistribution = await query
            .Where(sj => sj.IsTensionInKg)  // Solo los que usan kg para simplificar
            .GroupBy(sj => new
            {
                Range = sj.MainTension < 20 ? "Menos de 20 kg" :
                       sj.MainTension < 22 ? "20-21.9 kg" :
                       sj.MainTension < 24 ? "22-23.9 kg" :
                       sj.MainTension < 26 ? "24-25.9 kg" :
                       sj.MainTension < 28 ? "26-27.9 kg" :
                       "28 kg o más"
            })
            .Select(g => new
            {
                Range = g.Key.Range,
                Count = g.Count()
            })
            .ToListAsync();

        // Distribución por marcas de cuerdas principales
        var stringBrandDistribution = await query
            .Where(sj => sj.MainStringId.HasValue)
            .Join(
                _context.StringTypes,
                sj => sj.MainStringId,
                st => st.Id,
                (sj, st) => new { StringJob = sj, StringType = st }
            )
            .GroupBy(x => x.StringType.Brand)
            .Select(g => new
            {
                Brand = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();

        return new
        {
            StatusDistribution = statusDistribution,
            TensionDistribution = tensionDistribution,
            StringBrandDistribution = stringBrandDistribution
        };
    }
}