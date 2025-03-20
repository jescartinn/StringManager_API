using StringManager_API.DTOs;

namespace StringManager_API.Services;

public interface IStringTypeService : IBaseService<StringTypeDto, CreateStringTypeDto, UpdateStringTypeDto>
{
    // Métodos específicos para tipos de cuerdas si los necesitas en el futuro
}