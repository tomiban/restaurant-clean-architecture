using AutoMapper;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Users.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserInfoDto>();
    }
}