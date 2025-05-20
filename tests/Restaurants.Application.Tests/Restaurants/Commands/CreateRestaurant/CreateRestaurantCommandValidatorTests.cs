using FluentValidation.TestHelper;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostalCode = "3080"
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Category = "Italiano papu",
            ContactEmail = "test",
            PostalCode = "308032"
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Japanese")]
    [InlineData("Argentinian")]
    [InlineData("American")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        // Arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { Category = category };
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }

    [Theory()]
    [InlineData("102202")]
    [InlineData("102-202")]
    [InlineData("102 202")]
    [InlineData("102-20-2")]
    public void Validator_ForInvalidPostalCode_ShoulsHaveValidationErrorForPostalCode(string postalCode)
    {
        // Arrange
        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { PostalCode = postalCode };
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}