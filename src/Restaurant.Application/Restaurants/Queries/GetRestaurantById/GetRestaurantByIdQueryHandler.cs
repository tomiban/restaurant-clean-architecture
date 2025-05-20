using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Interfaces;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(
    ILogger<GetRestaurantByIdQuery> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IBlobStorageService storageService) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant {restaurantId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                         ?? throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.Id.ToString());
        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        restaurantDto.LogoSasUrl = storageService.GetBlobSasUrl(restaurant.LogoUrl);
        return restaurantDto;
    }
}