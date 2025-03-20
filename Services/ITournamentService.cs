using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface ITournamentService : IBaseService<TournamentDto, CreateTournamentDto, UpdateTournamentDto>
{
    Task<TournamentDto?> GetCurrentTournamentAsync();
}