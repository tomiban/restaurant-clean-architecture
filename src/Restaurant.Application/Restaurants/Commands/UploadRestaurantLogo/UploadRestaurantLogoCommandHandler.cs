using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Interfaces;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommandHandler(
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading restaurant logo for restaurant with id {restaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Domain.Entities.Restaurant), request.RestaurantId.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();
        var logoUrl = await blobStorageService.UploadToBlobAsync(request.FileName, request.File);
        restaurant.LogoUrl = logoUrl;
        await restaurantsRepository.SaveChanges();
    }
}