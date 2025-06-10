namespace StringManager_API.DTOs;

public class StringJobDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public PlayerDto? Player { get; set; }
    
    public int RacquetId { get; set; }
    public RacquetDto? Racquet { get; set; }
    
    public int? MainStringId { get; set; }
    public StringTypeDto? MainString { get; set; }
    
    public int? CrossStringId { get; set; }
    public StringTypeDto? CrossString { get; set; }
    
    public int? StringerId { get; set; }
    public StringerDto? Stringer { get; set; }
    
    public int? TournamentId { get; set; }
    public TournamentDto? Tournament { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? DueDate { get; set; }
    
    public double MainTension { get; set; }
    public double? CrossTension { get; set; }
    public bool IsTensionInKg { get; set; }
    public string? Logo { get; set; }
    
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    public int? Priority { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
}

public class CreateStringJobDto
{
    public int PlayerId { get; set; }
    public int RacquetId { get; set; }
    public int? MainStringId { get; set; }
    public int? CrossStringId { get; set; }
    public int? StringerId { get; set; }
    public int? TournamentId { get; set; }
    
    public double MainTension { get; set; }
    public double? CrossTension { get; set; }
    public bool IsTensionInKg { get; set; } = true;
    public string? Logo { get; set; }
    public DateTime? DueDate { get; set; }
    
    public string? Notes { get; set; }
    public int? Priority { get; set; }
    public decimal Price { get; set; } = 25.0m;
    public bool IsPaid { get; set; } = false;
}

public class UpdateStringJobDto
{
    public int? MainStringId { get; set; }
    public int? CrossStringId { get; set; }
    public int? StringerId { get; set; }
    
    public double MainTension { get; set; }
    public double? CrossTension { get; set; }
    public bool IsTensionInKg { get; set; } = true;
    public string? Logo { get; set; }
    public DateTime? DueDate { get; set; }
    
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    public int? Priority { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
}