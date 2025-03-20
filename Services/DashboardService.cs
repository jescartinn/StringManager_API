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
        var currentTournament = await _context.Tournaments
            .Where(t => t.StartDate <= today && t.EndDate >= today)
            .OrderBy(t => t.StartDate)
            .FirstOrDefaultAsync();

        int totalPendingJobs = await _context.StringJobs
            .Where(sj => sj.Status == "Pending")
            .CountAsync();

        int totalInProgressJobs = await _context.StringJobs
            .Where(sj => sj.Status == "InProgress")
            .CountAsync();

        int totalCompletedJobsToday = await _context.StringJobs
            .Where(sj => sj.Status == "Completed" &&
                  sj.CompletedAt.HasValue &&
                  sj.CompletedAt.Value.Date == today)
            .CountAsync();

        int highPriorityJobs = await _context.StringJobs
            .Where(sj => (sj.Status == "Pending" || sj.Status == "InProgress") &&
                  sj.Priority == 1)
            .CountAsync();

        var topStringers = await _context.StringJobs
            .Where(sj => sj.Status == "Completed" &&
                  sj.CompletedAt.HasValue &&
                  sj.StringerId.HasValue)
            .GroupBy(sj => sj.StringerId)
            .Select(g => new
            {
                StringerId = g.Key,
                StringerName = _context.Stringers
                    .Where(s => s.Id == g.Key)
                    .Select(s => s.Name + " " + s.LastName)
                    .FirstOrDefault(),
                CompletedJobs = g.Count()
            })
            .OrderByDescending(x => x.CompletedJobs)
            .Take(5)
            .ToListAsync();

        var topPlayers = await _context.StringJobs
            .GroupBy(sj => sj.PlayerId)
            .Select(g => new
            {
                PlayerId = g.Key,
                PlayerName = _context.Players
                    .Where(p => p.Id == g.Key)
                    .Select(p => p.Name + " " + p.LastName)
                    .FirstOrDefault(),
                TotalJobs = g.Count()
            })
            .OrderByDescending(x => x.TotalJobs)
            .Take(5)
            .ToListAsync();

        var topStrings = await _context.StringJobs
            .Where(sj => sj.MainStringId.HasValue)
            .GroupBy(sj => sj.MainStringId)
            .Select(g => new
            {
                StringId = g.Key,
                StringName = _context.StringTypes
                    .Where(st => st.Id == g.Key)
                    .Select(st => st.Brand + " " + st.Model)
                    .FirstOrDefault(),
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
            PendingJobs = totalPendingJobs,
            InProgressJobs = totalInProgressJobs,
            CompletedJobsToday = totalCompletedJobsToday,
            HighPriorityJobs = highPriorityJobs,
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
            .GroupBy(sj => _context.StringTypes
                .Where(st => st.Id == sj.MainStringId)
                .Select(st => st.Brand)
                .FirstOrDefault())
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