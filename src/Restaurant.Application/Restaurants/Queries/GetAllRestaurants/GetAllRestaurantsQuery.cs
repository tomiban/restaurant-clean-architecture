using System.ComponentModel.DataAnnotations;
using MediatR;
using Restaurant.Application.Common;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Domain.Constants;
using Restaurants.Domain.Constants;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery() : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; set; }

    [Required] public int PageNumber { get; set; }

    [Required] public int PageSize { get; set; }

    public RestaurantSortBy? SortBy { get; set; }

    public SortDirection? SortDirection { get; set; }
}