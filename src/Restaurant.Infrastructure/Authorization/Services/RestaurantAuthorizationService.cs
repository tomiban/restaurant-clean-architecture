using Microsoft.Extensions.Logging;
using Restaurant.Application.Users;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext)
    : IRestaurantAuthorizationService
{
    public bool Authorize(Domain.Entities.Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
            currentUser.Email, resourceOperation, restaurant.Name);

        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/read operation - successful auth");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Delete operation - successful auth");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) &&
            currentUser.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant Owner operation - successful auth");
            return true;
        }

        return false;
    }
}