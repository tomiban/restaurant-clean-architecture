using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Exceptions;
using Restaurant.Domain.Interfaces;
using Restaurant.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTest
{
    private readonly UpdateRestaurantCommandHandler _handler;
    private readonly Mock<IRestaurantAuthorizationService> _mockAuthorizationService;
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRestaurantsRepository> _mockRestaurantsRepository;

    public UpdateRestaurantCommandHandlerTest()
    {
        _mockLogger = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mockRestaurantsRepository = new Mock<IRestaurantsRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockAuthorizationService = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(_mockLogger.Object, _mockMapper.Object,
            _mockRestaurantsRepository.Object, _mockAuthorizationService.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        // Arrange

        var restaurantId = 1;

        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Updated Name",
            Description = "Updated Description",
            Category = "Japanese",
            HasDelivery = true,
        };

        var restaurant = new Restaurant.Domain.Entities.Restaurant()
        {
            Id = restaurantId, Name = "Old Name", Description = "Old Description", Category = "Old Category",
            HasDelivery = false
        };

        _mockRestaurantsRepository
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(restaurant);

        _mockAuthorizationService
            .Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        _mockMapper
            .Setup(mapper => mapper.Map(command, restaurant));

        _mockRestaurantsRepository
            .Setup(repo => repo.SaveChanges())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(restaurant);

        _mockRestaurantsRepository.Verify(repo => repo.GetByIdAsync(command.Id));
        _mockAuthorizationService.Verify(auth => auth.Authorize(restaurant, ResourceOperation.Update));
        _mockMapper.Verify(mapper => mapper.Map(command, restaurant));
        _mockRestaurantsRepository.Verify(repo => repo.SaveChanges());
    }

    [Fact]
    public async Task Handle_WithValidRequest_ThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Updated Name",
            Description = "Updated Description",
            Category = "Japanese",
            HasDelivery = true,
        };

        var restaurant = new Restaurant.Domain.Entities.Restaurant()
        {
            Id = restaurantId, Name = "Old Name", Description = "Old Description", Category = "Old Category",
            HasDelivery = false
        };

        _mockRestaurantsRepository
            .Setup(repo => repo.GetByIdAsync(command.Id))
            .ReturnsAsync(restaurant);

        _mockAuthorizationService
            .Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        _mockMapper
            .Setup(mapper => mapper.Map(command, restaurant));

        _mockRestaurantsRepository
            .Setup(repo => repo.SaveChanges())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(restaurant);

        _mockRestaurantsRepository.Verify(repo => repo.GetByIdAsync(command.Id));
        _mockAuthorizationService.Verify(auth => auth.Authorize(restaurant, ResourceOperation.Update));
        _mockMapper.Verify(mapper => mapper.Map(command, restaurant));
        _mockRestaurantsRepository.Verify(repo => repo.SaveChanges());
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowForbidException()
    {
        // Arrange
        const int restaurantId = 1;
        var request = new UpdateRestaurantCommand() { Id = restaurantId, };
        var existingRestaurant = new Restaurant.Domain.Entities.Restaurant() { Id = restaurantId };

        _mockRestaurantsRepository
            .Setup(repo => repo.GetByIdAsync(request.Id))
            .ReturnsAsync(existingRestaurant);

        _mockAuthorizationService
            .Setup(auth => auth.Authorize(existingRestaurant, ResourceOperation.Update))
            .Returns(false);

        // Act
        Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ForbidException>();
    }
}