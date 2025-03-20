using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface IStringJobService : IBaseService<StringJobDto, CreateStringJobDto, UpdateStringJobDto>
{
    Task<IEnumerable<StringJobDto>> GetByStatusAsync(string status);
    Task<IEnumerable<StringJobDto>> GetByTournamentIdAsync(int tournamentId);
    Task<IEnumerable<StringJobDto>> GetByPlayerIdAsync(int playerId);
    Task<IEnumerable<StringJobDto>> GetByStringerIdAsync(int stringerId);
    Task<bool> CompleteJobAsync(int id, CompleteStringJobDto completeDto);
    Task<bool> CancelJobAsync(int id, string? cancelReason);
    Task<bool> StartJobAsync(int id);
}