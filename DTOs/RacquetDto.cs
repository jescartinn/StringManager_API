namespace StringManager_API.DTOs;

public class RacquetDto
{
    public int RacquetId { get; set; }
    public int PlayerId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public double? HeadSize { get; set; }
    public string? Notes { get; set; }
    public PlayerDto? Player { get; set; }
}

public class CreateRacquetDto
{
    public int PlayerId { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public double? HeadSize { get; set; }
    public string? Notes { get; set; }
}

public class UpdateRacquetDto
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public double? HeadSize { get; set; }
    public string? Notes { get; set; }
}