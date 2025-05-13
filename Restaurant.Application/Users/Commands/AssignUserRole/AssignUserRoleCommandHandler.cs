using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
    ILogger<AssignUserRoleCommand> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning role {RoleName} to user with email {UserEmail}",
            request.RoleName, request.UserEmail);
        var user = await userManager.FindByEmailAsync(request.UserEmail) ??
                   throw new NotFoundException(nameof(User), request.UserEmail);
        var role = await roleManager.FindByNameAsync(request.RoleName) ??
                   throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        await userManager.AddToRoleAsync(user, role.Name!);
    }
}