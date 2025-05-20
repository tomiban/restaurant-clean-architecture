using MediatR;

namespace Restaurant.Application.Dishes.Commands.CreateDish;

public class CreateDishCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }
    public int RestaurantId { get; set; }
}