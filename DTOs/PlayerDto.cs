namespace StringManager_API.DTOs;

public class PlayerDto
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
}

public class CreatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
}

public class UpdatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CountryCode { get; set; }
}