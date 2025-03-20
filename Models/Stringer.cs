namespace StringManager_API.Models;

public class Stringer
{
    public int StringerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<StringJob> StringJobs { get; set; } = new List<StringJob>();
}