using MediatR;
using Restaurant.Application.Dishes.Dtos;

namespace Restaurant.Application.Dishes.Queries.GetDishesForRestaurant;

public class GetDishesForRestaurantQuery(int restaurantId) : IRequest<IEnumerable<DishDto>>
{
    public int RestaurantId { get; set; } = restaurantId;
}