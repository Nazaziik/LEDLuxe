using AutoMapper;
using LEDLuxe.Application.DTOs.ViewModels;
using LEDLuxe.Core.Entities.Users;

namespace LEDLuxe.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}