using MediatR;
using Restaurant.Application.Users.Dtos;

namespace Restaurant.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQuery : IRequest<UserInfoDto>
{
}