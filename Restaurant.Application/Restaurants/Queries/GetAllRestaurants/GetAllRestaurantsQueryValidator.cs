using FluentValidation;
using Restaurants.Domain.Constants;

namespace Restaurant.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private RestaurantSortBy[] allowedSortByValues =
        [RestaurantSortBy.Name, RestaurantSortBy.Category, RestaurantSortBy.Description];

    private int[] allowPageSize = [5, 10, 25, 50, 100];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowPageSize.Contains(value))
            .WithMessage($"Page size must be in [{string.Join(", ", allowPageSize)}]");

        RuleFor(r => r.SortBy)
            .Must(value => value == null || allowedSortByValues.Contains(value.Value))
            .WithMessage($"Sort by is optional, or must be in [{string.Join(", ", allowedSortByValues)}]");
    }
}