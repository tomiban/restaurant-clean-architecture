using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Users;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommand> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext
) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand createRestaurantCommand, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("{UserEmail} {UserId} is creating a new restaurant {@Restaurant}", currentUser.Email,
            currentUser.Id, createRestaurantCommand);
        var restaurant = mapper.Map<Domain.Entities.Restaurant>(createRestaurantCommand);
        restaurant.OwnerId = currentUser.Id;
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}