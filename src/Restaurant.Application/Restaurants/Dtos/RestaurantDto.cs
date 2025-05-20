using Restaurant.Application.Dishes.Dtos;

namespace Restaurant.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? LogoSasUrl { get; set; }

    public List<DishDto> Dishes { get; set; } = [];
}

// public static RestaurantDto? FromEntity(Domain.Entities.Restaurant? restaurant)
// {
//     if (restaurant is null) return null;
//     return new RestaurantDto()
//     {
//         Id = restaurant.Id,
//         Name = restaurant.Name,
//         Description = restaurant.Description,
//         Category = restaurant.Category,
//         HasDelivery = restaurant.HasDelivery,
//         City = restaurant.Address?.City,
//         PostalCode = restaurant.Address?.PostalCode,
//         Street = restaurant.Address?.Street,
//         Dishes = restaurant.Dishes.Select( DishDto.FromEntity).ToList()
//     };
// }