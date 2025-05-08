using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelsController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpPost]
    public async Task<ActionResult<LabelDto>> GenerateLabel(GenerateLabelDto generateLabelDto)
    {
        try
        {
            var label = await _labelService.GenerateLabelAsync(generateLabelDto);
            return Ok(label);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error generating label: {ex.Message}");
        }
    }

    [HttpGet("{jobId}/qrcode")]
    public async Task<ActionResult<string>> GenerateQRCodeData(int jobId)
    {
        try
        {
            var qrCodeData = await _labelService.GenerateQRCodeDataAsync(jobId);
            return Ok(qrCodeData);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error generating QR code data: {ex.Message}");
        }
    }
}