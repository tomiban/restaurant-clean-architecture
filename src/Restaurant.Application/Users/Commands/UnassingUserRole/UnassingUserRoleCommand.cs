using MediatR;

namespace Restaurant.Application.Users.Commands.UnassingUserRole;

public class UnassingUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}