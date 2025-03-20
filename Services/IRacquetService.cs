using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface IRacquetService : IBaseService<RacquetDto, CreateRacquetDto, UpdateRacquetDto>
{
    Task<IEnumerable<RacquetDto>> GetByPlayerIdAsync(int playerId);
}