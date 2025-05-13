using MediatR;
using Restaurant.Application.Restaurants.Dtos;

namespace Restaurant.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto>
{
    public int Id { get; } = id; // permite inicializar la propiedad al instanciar por primera vez
}