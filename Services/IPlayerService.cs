using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface IPlayerService : IBaseService<PlayerDto, CreatePlayerDto, UpdatePlayerDto>
{
    // Métodos específicos para jugadores si se necesitan en el futuro
}