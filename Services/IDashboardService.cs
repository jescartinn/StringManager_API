namespace StringManager_API.Services;

public interface IDashboardService
{
    Task<object> GetDashboardStatsAsync();
    Task<object> GetDistributionStatsAsync(int? tournamentId = null);
}