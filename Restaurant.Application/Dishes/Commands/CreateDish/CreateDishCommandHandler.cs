using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper) : IRequestHandler<CreateDishCommand, int>
{
    public Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish {@Dish}", request);
        var restaurant = restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        var dish = mapper.Map<Dish>(request);
        return dishesRepository.CreateAsync(dish);
    }
}