namespace StringManager_API.Services;

// Interfaz genérica para operaciones CRUD básicas
public interface IBaseService<T, TCreateDto, TUpdateDto> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(TCreateDto createDto);
    Task<bool> UpdateAsync(int id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}