using MediatR;
using Restaurant.Application.Restaurants.Dtos;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery() : IRequest<IEnumerable<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}