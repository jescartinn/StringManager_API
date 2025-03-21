using StringManager_API.DTOs;
using StringManager_API.Models;

namespace StringManager_API.Services;

public interface IUserService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<bool> UpdateUserAsync(int userId, UserDto userDto);
    Task<bool> DeleteUserAsync(int userId);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}