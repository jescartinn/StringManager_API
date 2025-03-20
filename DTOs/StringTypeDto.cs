namespace StringManager_API.DTOs;

public class StringTypeDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Gauge { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
}

public class CreateStringTypeDto
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Gauge { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
}

public class UpdateStringTypeDto
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? Gauge { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
}