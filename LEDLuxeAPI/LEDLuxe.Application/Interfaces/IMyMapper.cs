using LEDLuxe.Application.DTOs.InputModels;
using LEDLuxe.Application.DTOs.ViewModels;
using LEDLuxe.Core.Entities.Users;

namespace LEDLuxe.Application.Interfaces;

public interface IMyMapper
{
    UserDto MapToUserDto(User user);

    User MapToUserEntity(UserRegistrationDto registrationDto);
}