using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;
using System.Security.Claims;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest();
        }

        var result = await _userService.UpdateUserAsync(id, userDto);

        if (!result)
        {
            return BadRequest("No se pudo actualizar el usuario. Es posible que el nombre de usuario o correo electrónico ya estén en uso.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        var result = await _userService.CreateUserAsync(createUserDto);

        if (result == null)
        {
            return BadRequest("Error al crear el usuario. Verifique que los datos sean correctos.");
        }

        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
    }

    [HttpPost("{id}/change-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserPassword(int id, ChangeUserPasswordDto changePasswordDto)
    {
        var result = await _userService.ChangeUserPasswordAsync(id, changePasswordDto.NewPassword);

        if (!result)
        {
            return BadRequest("No se pudo cambiar la contraseña del usuario.");
        }

        return NoContent();
    }

    [HttpPatch("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole(int id, ChangeUserRoleDto changeRoleDto)
    {
        var result = await _userService.ChangeUserRoleAsync(id, changeRoleDto.Role);

        if (!result)
        {
            return BadRequest("No se pudo cambiar el rol del usuario.");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId == id.ToString())
        {
            return Ok(new { requiresTokenRefresh = true });
        }

        return NoContent();
    }
}