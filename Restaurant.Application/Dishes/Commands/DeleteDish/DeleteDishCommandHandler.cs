using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommandHandler(
    ILogger<DeleteDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting dish with id {DishId} for restaurant with id {RestaurantId}",
            request.RestaurantId, request.DishId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.RestaurantId.ToString());
        var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == request.DishId)
                   ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        await dishesRepository.DeleteAsync(dish);
    }
}