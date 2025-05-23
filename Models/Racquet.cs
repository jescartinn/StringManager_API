namespace StringManager_API.Models;

public class Racquet
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public Player? Player { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public double? HeadSize { get; set; }
    public string? Notes { get; set; }
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}