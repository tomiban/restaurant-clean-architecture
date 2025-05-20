using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurant.Application.Users;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Authorization.Requirements;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements;

[TestSubject(typeof(CreatedMultipleRequirementHandler))]
public class CreatedMultipleRequirementHandlerTest
{
    private readonly CreatedMultipleRequirementHandler _handler;
    private readonly Mock _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;

    public CreatedMultipleRequirementHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreatedMultipleRequirementHandler>>();
        _userContextMock = new Mock<IUserContext>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        _handler = new CreatedMultipleRequirementHandler(
            (ILogger<CreatedMultipleRequirementHandler>)_loggerMock.Object,
            _userContextMock.Object,
            _restaurantsRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        var requirement = new CreatedMultipleRequirement(2);
        var restaurants = new List<Restaurant.Domain.Entities.Restaurant>
        {
            new() { OwnerId = "1" },
            new() { OwnerId = "1" }
        };

        var user = new CurrentUser("1", "test@example.com", Array.Empty<string>(), "Nationality", null);

        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        _restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }


    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        var requirement = new CreatedMultipleRequirement(2);
        var restaurants = new List<Restaurant.Domain.Entities.Restaurant>
        {
            new() { OwnerId = "1" },
            new() { OwnerId = "2" }
        };

        var user = new CurrentUser("1", "test@example.com", Array.Empty<string>(), "Nationality", null);

        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(user);
        _restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);
        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}