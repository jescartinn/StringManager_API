namespace StringManager_API.DTOs;

public class LabelDto
{
    public int JobId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerLastName { get; set; } = string.Empty;
    public string RacquetBrand { get; set; } = string.Empty;
    public string RacquetModel { get; set; } = string.Empty;
    public string StringBrand { get; set; } = string.Empty;
    public string StringModel { get; set; } = string.Empty;
    public string? CrossStringBrand { get; set; }
    public string? CrossStringModel { get; set; }
    public double MainTension { get; set; }
    public double? CrossTension { get; set; }
    public bool IsTensionInKg { get; set; }
    public string DateCompleted { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? QRCodeData { get; set; }
}

public class GenerateLabelDto
{
    public int JobId { get; set; }
    public bool IncludePlayerInfo { get; set; } = true;
    public bool IncludeRacquetInfo { get; set; } = true;
    public bool IncludeStringInfo { get; set; } = true;
    public bool IncludeTensionInfo { get; set; } = true;
    public bool IncludeDateInfo { get; set; } = true;
    public bool IncludeLogo { get; set; } = true;
    public bool GenerateQRCode { get; set; } = true;
    public string LabelSize { get; set; } = "medium"; // small, medium, large
}