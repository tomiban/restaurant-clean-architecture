using FluentValidation;

namespace Restaurant.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dish => dish.Name).NotEmpty();
        RuleFor(dish => dish.Price).NotEmpty().GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non negative number");
        RuleFor(dish => dish.KiloCalories).GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a non negative number");
    }
}