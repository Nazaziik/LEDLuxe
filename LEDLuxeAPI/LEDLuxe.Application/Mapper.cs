using AutoMapper;
using LEDLuxe.Application.DTOs.InputModels;
using LEDLuxe.Application.DTOs.ViewModels;
using LEDLuxe.Application.Interfaces;
using LEDLuxe.Core.Entities.Users;

namespace LEDLuxe.Application;

public class Mapper(IMapper mapper) : IMyMapper
{
    public UserDto MapToUserDto(User user)
    {
        return mapper.Map<User, UserDto>(user);
    }

    public User MapToUserEntity(UserRegistrationDto registrationDto)
    {
        return mapper.Map<UserRegistrationDto, User>(registrationDto);
    }
}