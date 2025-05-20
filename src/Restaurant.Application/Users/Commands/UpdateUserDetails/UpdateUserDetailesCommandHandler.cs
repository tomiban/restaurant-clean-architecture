using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;

namespace Restaurant.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailesCommandHandler(
    ILogger<UpdateUserDetailesCommandHandler> logger,
    IUserContext userContext,
    IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user details {@Request}  for user with id {userId}", request, user!.Id);

        var dbUser = await userStore.FindByIdAsync(user.Id!, cancellationToken);

        if (dbUser is null) throw new NotFoundException(nameof(User), user.Id!.ToString());

        dbUser.DateOfBirth = request.DateOfBirth;
        dbUser.Nationality = request.Nationality;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}