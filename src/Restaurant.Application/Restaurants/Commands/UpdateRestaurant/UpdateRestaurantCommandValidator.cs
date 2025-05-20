using FluentValidation;

namespace Restaurant.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    private static readonly List<string> ValidCategories =
        ["Italian", "Argentinian", "Mexican", "Japanese", "American"];

    public UpdateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Id)
            .GreaterThan(0).WithMessage("Invalid restaurant ID");

        // Validaciones condicionales solo si se proporciona un valor
        When(dto => dto.Name != null, () =>
        {
            RuleFor(dto => dto.Name)
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters");
        });

        When(dto => dto.Category != null, () =>
        {
            RuleFor(dto => dto.Category)
                .Must(category => ValidCategories.Contains(category!))
                .WithMessage($"Category must be one of: {string.Join(", ", ValidCategories)}");
        });

        When(dto => dto.ContactEmail != null, () =>
        {
            RuleFor(dto => dto.ContactEmail)
                .EmailAddress().WithMessage("Invalid email format");
        });

        When(dto => dto.ContactNumber != null, () =>
        {
            RuleFor(dto => dto.ContactNumber)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format");
        });

        When(dto => dto.PostalCode != null, () =>
        {
            RuleFor(dto => dto.PostalCode)
                .Matches(@"^\d{4}$").WithMessage("Invalid postal code format (must be 4 digits)");
        });
    }
}