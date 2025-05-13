using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    ILogger<CreateRestaurantCommand> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand createRestaurantCommand, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating restaurant {@Restaurant}", createRestaurantCommand);
        var restaurant = mapper.Map<Domain.Entities.Restaurant>(createRestaurantCommand);
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}