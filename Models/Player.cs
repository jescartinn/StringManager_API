namespace StringManager_API.Models;

public class Player
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
    public ICollection<Racquet> Racquets { get; set; } = new List<Racquet>();
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}