using Microsoft.AspNetCore.Mvc;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // GET: api/Dashboard/stats
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetDashboardStats()
    {
        var stats = await _dashboardService.GetDashboardStatsAsync();
        return Ok(stats);
    }

    // GET: api/Dashboard/distribution
    [HttpGet("distribution")]
    public async Task<ActionResult<object>> GetDistributionStats([FromQuery] int? tournamentId = null)
    {
        var distribution = await _dashboardService.GetDistributionStatsAsync(tournamentId);
        return Ok(distribution);
    }
}