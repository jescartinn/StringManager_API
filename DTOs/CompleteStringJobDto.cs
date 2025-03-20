namespace StringManager_API.DTOs;

public class CompleteStringJobDto
{
    public DateTime CompletedAt { get; set; } = DateTime.Now;
    public string? Notes { get; set; }
}