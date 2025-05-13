using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Users.Commands.UnassingUserRole;

public class UnassingUserRoleCommandHandler(
    ILogger<UnassingUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassingUserRoleCommand>
{
    public async Task Handle(UnassingUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning role {RoleName} to user with email {UserEmail}", request.RoleName,
            request.UserEmail);
        var user = await userManager.FindByEmailAsync(request.UserEmail) ??
                   throw new NotFoundException(nameof(User), request.UserEmail);
        var role = await roleManager.FindByNameAsync(request.RoleName) ??
                   throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        await userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}