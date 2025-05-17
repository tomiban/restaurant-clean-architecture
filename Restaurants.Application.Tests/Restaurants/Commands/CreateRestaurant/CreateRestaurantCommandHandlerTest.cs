using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Users;
using Restaurant.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandHandler))]
public class CreateRestaurantCommandHandlerTest
{
    private readonly Mock<ILogger<CreateRestaurantCommand>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;

    public CreateRestaurantCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreateRestaurantCommand>>();
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();
    }

    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreateRestaurantId()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "12345"
        };

        var restaurant = new Restaurant.Domain.Entities.Restaurant();

        _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Restaurant.Domain.Entities.Restaurant>()))
            .ReturnsAsync(1);
        _mapperMock.Setup(r => r.Map<Restaurant.Domain.Entities.Restaurant>(command)).Returns(restaurant);


        var commandHandler = new CreateRestaurantCommandHandler(_loggerMock.Object, _mapperMock.Object,
            _repositoryMock.Object, _userContextMock.Object);
        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        _repositoryMock.Verify(r => r.CreateAsync(restaurant));
    }
}