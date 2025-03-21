using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StringManager_API.DTOs;
using StringManager_API.Services;
using System.Security.Claims;

namespace StringManager_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var result = await _userService.RegisterAsync(registerDto);

        if (result == null)
        {
            return BadRequest("Error al registrar el usuario. El nombre de usuario o correo electrónico ya están en uso o las contraseñas no coinciden.");
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var result = await _userService.LoginAsync(loginDto);

        if (result == null)
        {
            return Unauthorized("Credenciales inválidas.");
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
        {
            return Unauthorized();
        }

        var result = await _userService.ChangePasswordAsync(id, changePasswordDto);

        if (!result)
        {
            return BadRequest("No se pudo cambiar la contraseña. Verifique que la contraseña actual sea correcta y que las nuevas contraseñas coincidan.");
        }

        return NoContent();
    }
}