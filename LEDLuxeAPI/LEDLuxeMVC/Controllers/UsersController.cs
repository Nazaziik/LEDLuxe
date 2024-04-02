using LEDLuxe.Application.DTOs.InputModels;
using LEDLuxe.Application.DTOs.ViewModels;
using LEDLuxe.Application.Interfaces;
using LEDLuxe.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LEDLuxe.Web.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService, IMyMapper mapper) : ControllerBase
{
    private readonly IUserService _userService = userService ??
        throw new ArgumentNullException(nameof(userService));

    private readonly IMyMapper _mapper = mapper ??
        throw new ArgumentNullException(nameof(mapper));

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        var userDto = _mapper.MapToUserDto(user);
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserRegistrationDto registrationDto)
    {
        var user = _mapper.MapToUserEntity(registrationDto);
        await _userService.CreateUserAsync(user.Email, user.PasswordHash);

        var userDto = _mapper.MapToUserDto(user);
        return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
    }
}