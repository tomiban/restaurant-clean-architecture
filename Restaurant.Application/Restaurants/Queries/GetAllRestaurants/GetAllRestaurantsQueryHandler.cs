using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurant.Application.Common;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Domain.Repositories;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    ILogger<GetAllRestaurantsQuery> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting all restaurants with filters: SearchPhrase='{SearchPhrase}', PageSize={PageSize}, PageNumber={PageNumber}, SortBy='{SortBy}', SortDirection='{SortDirection}'",
            request.SearchPhrase, request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);

        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(
            request.SearchPhrase,
            request.PageSize,
            request.PageNumber,
            request.SortBy?.ToString(),
            request.SortDirection);

        logger.LogInformation("Found {Count} restaurants matching criteria", restaurants.Count);

        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        var result = new PagedResult<RestaurantDto>(restaurantsDto, totalCount, request.PageSize, request.PageNumber);

        return result;
    }
}