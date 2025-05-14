using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Users;

namespace Restaurant.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, IUserContext userContext)
    : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling Minimum Age requirement",
            currentUser.Email, currentUser.DateOfBirth);

        if (currentUser.DateOfBirth == null)
        {
            logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("User: {Email} is of legal age", currentUser.Email);
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}