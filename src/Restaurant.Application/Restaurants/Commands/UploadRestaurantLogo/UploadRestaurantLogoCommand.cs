using MediatR;

namespace Restaurant.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest
{
    public int RestaurantId { get; set; }
    public required string FileName { get; set; }
    public required Stream File { get; set; }
}