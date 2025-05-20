using MediatR;
using Restaurant.Application.Dishes.Dtos;

namespace Restaurant.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; set; } = restaurantId;
    public int DishId { get; set; } = dishId;
}