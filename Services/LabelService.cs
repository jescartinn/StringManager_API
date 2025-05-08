using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StringManager_API.Data;
using StringManager_API.DTOs;

namespace StringManager_API.Services;

public class LabelService : ILabelService
{
    private readonly ApplicationDbContext _context;

    public LabelService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LabelDto> GenerateLabelAsync(GenerateLabelDto generateLabelDto)
    {
        // Get the string job with all needed relationships
        var job = await _context.StringJobs
            .Include(j => j.Player)
            .Include(j => j.Racquet)
            .Include(j => j.MainString)
            .Include(j => j.CrossString)
            .Where(j => j.Id == generateLabelDto.JobId)
            .FirstOrDefaultAsync();

        if (job == null)
        {
            throw new InvalidOperationException($"Job with ID {generateLabelDto.JobId} not found");
        }

        // Create the label DTO
        var labelDto = new LabelDto
        {
            JobId = job.Id,
            PlayerName = job.Player?.Name ?? "Unknown",
            PlayerLastName = job.Player?.LastName ?? "",
            RacquetBrand = job.Racquet?.Brand ?? "Unknown",
            RacquetModel = job.Racquet?.Model ?? "",
            StringBrand = job.MainString?.Brand ?? "Unknown",
            StringModel = job.MainString?.Model ?? "",
            CrossStringBrand = job.CrossString?.Brand,
            CrossStringModel = job.CrossString?.Model,
            MainTension = job.MainTension,
            CrossTension = job.CrossTension,
            IsTensionInKg = job.IsTensionInKg,
            DateCompleted = job.CompletedAt?.ToString("yyyy-MM-dd") ?? job.CreatedAt.ToString("yyyy-MM-dd"),
            Logo = job.Logo
        };

        // Generate QR code data if requested
        if (generateLabelDto.GenerateQRCode)
        {
            labelDto.QRCodeData = await GenerateQRCodeDataAsync(job.Id);
        }

        return labelDto;
    }

    public async Task<string> GenerateQRCodeDataAsync(int jobId)
    {
        // Get the string job with all needed relationships
        var job = await _context.StringJobs
            .Include(j => j.Player)
            .Include(j => j.Racquet)
            .Include(j => j.MainString)
            .Include(j => j.CrossString)
            .Where(j => j.Id == jobId)
            .FirstOrDefaultAsync();

        if (job == null)
        {
            throw new InvalidOperationException($"Job with ID {jobId} not found");
        }

        // Create a data object for the QR code
        var qrCodeData = new
        {
            Id = job.Id,
            Player = job.Player != null ? $"{job.Player.Name} {job.Player.LastName}" : "Unknown",
            Racquet = job.Racquet != null ? $"{job.Racquet.Brand} {job.Racquet.Model}" : "Unknown",
            MainString = job.MainString != null ? $"{job.MainString.Brand} {job.MainString.Model}" : "Unknown",
            CrossString = job.CrossString != null && job.CrossString.Id != job.MainStringId 
                ? $"{job.CrossString.Brand} {job.CrossString.Model}" 
                : null,
            Tension = FormatTension(job.MainTension, job.CrossTension, job.IsTensionInKg),
            Date = job.CompletedAt?.ToString("yyyy-MM-dd") ?? job.CreatedAt.ToString("yyyy-MM-dd")
        };

        // Serialize to JSON
        return JsonSerializer.Serialize(qrCodeData);
    }

    private string FormatTension(double mainTension, double? crossTension, bool isKg)
    {
        var unit = isKg ? "kg" : "lb";
        
        if (crossTension.HasValue && crossTension.Value != mainTension)
        {
            return $"{mainTension}/{crossTension.Value} {unit}";
        }
        
        return $"{mainTension} {unit}";
    }
}