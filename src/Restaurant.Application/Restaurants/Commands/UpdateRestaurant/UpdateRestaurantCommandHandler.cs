using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Interfaces;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<UpdateRestaurantCommand, Domain.Entities.Restaurant>
{
    public async Task<Domain.Entities.Restaurant> Handle(UpdateRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant  with Id {restaurantId} with {@restaurant}", request.Id, request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                         ?? throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.Id.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        mapper.Map(request, restaurant);

        await restaurantsRepository.SaveChanges();
        return restaurant;
    }
}