using FluentValidation;

namespace Restaurant.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Argentinian", "Mexican", "Japanese", "American"];

    public CreateRestaurantCommandValidator()
    {
        // Defiinimos las reglas del dto
        RuleFor(dto => dto.Name)
            .Length(3, 100);
        RuleFor(dto => dto.Category)
            .Must(category => validCategories.Contains(category))
            .WithMessage("Invalid category");
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description is required");
        RuleFor(dto => dto.Category)
            .NotEmpty().WithMessage("Category is required");
        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Invalid email");
        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number");
        RuleFor(dto => dto.PostalCode).Matches(@"^\d{4}$").WithMessage("Invalid postal code");
    }
}