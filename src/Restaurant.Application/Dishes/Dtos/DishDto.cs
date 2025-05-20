namespace Restaurant.Application.Dishes.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }


    public int RestaurantId { get; set; }
}

// public static DishDto FromEntity(Dish dish)
// {
//     return new DishDto()
//     {
//         Id = dish.Id,
//         Name = dish.Name,
//         Description = dish.Description,
//         Price = dish.Price,
//         KiloCalories = dish.KiloCalories 
//     };
// }