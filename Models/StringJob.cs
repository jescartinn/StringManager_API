namespace StringManager_API.Models;

public class StringJob
{
    public int StringJobId { get; set; }
    
    public int PlayerId { get; set; }
    public Player? Player { get; set; }
    
    public int RacquetId { get; set; }
    public Racquet? Racquet { get; set; }
    
    public int? MainStringId { get; set; }
    public StringType? MainString { get; set; }
    
    public int? CrossStringId { get; set; }
    public StringType? CrossString { get; set; }
    
    public int? StringerId { get; set; }
    public Stringer? Stringer { get; set; }
    
    public int? TournamentId { get; set; }
    public Tournament? Tournament { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
    
    public double MainTension { get; set; } // en kg o lb
    public double? CrossTension { get; set; } // si es diferente de MainTension
    public bool IsTensionInKg { get; set; } = true; // true = kg, false = lb
    
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    public string? Notes { get; set; }
    
    public int? Priority { get; set; } // 1 = alta, 2 = media, 3 = baja
}