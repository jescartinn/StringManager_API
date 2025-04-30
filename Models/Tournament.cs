namespace StringManager_API.Models;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}