namespace StringManager_API.Models;

public class Tournament
{
    public int TournamentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; } // Grand Slam, ATP 1000, etc.
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}