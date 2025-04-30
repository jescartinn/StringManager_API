namespace StringManager_API.Models;

public class StringType
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Gauge { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}