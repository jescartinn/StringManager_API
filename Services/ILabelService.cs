using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface ILabelService
{
    Task<LabelDto> GenerateLabelAsync(GenerateLabelDto generateLabelDto);
    Task<string> GenerateQRCodeDataAsync(int jobId);
}