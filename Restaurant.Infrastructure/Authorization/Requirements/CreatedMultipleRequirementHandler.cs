using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Users;
using Restaurant.Domain.Repositories;

namespace Restaurant.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRequirementHandler(
    ILogger<CreatedMultipleRequirementHandler> logger,
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<CreatedMultipleRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CreatedMultipleRequirement requirement)
    {
        logger.LogInformation("Handling CreatedMultipleRequirement");
        var user = userContext.GetCurrentUser();
        var restaurants = await restaurantsRepository.GetAllAsync();
        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == user!.Id);

        if (userRestaurantsCreated >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}