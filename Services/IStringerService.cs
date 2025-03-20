using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface IStringerService : IBaseService<StringerDto, CreateStringerDto, UpdateStringerDto>
{
    // Métodos específicos para encordadores si los necesitas en el futuro
}