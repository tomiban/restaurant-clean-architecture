using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

[TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTest
{
    private readonly IMapper _mapper;

    public RestaurantsProfileTest()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantsProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void CreateMap_ForRestaurantDtoToRestaurant_MapsCorrectly()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "A description",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "test@restaurant.com",
            ContactNumber = "123456789",
            City = "Paris",
            Street = "Main Street 123",
            PostalCode = "75000"
        };

        // Act
        var result = _mapper.Map<Restaurant.Domain.Entities.Restaurant>(command);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Category.Should().Be(command.Category);
        result.HasDelivery.Should().Be(command.HasDelivery);
        result.ContactEmail.Should().Be(command.ContactEmail);
        result.ContactNumber.Should().Be(command.ContactNumber);
        result.Address.Should().NotBeNull();
        result.Address.City.Should().Be(command.City);
        result.Address.Street.Should().Be(command.Street);
        result.Address.PostalCode.Should().Be(command.PostalCode);
    }

    [Fact]
    public void UpdateMap_ForRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Updated Restaurant",
            Description = "Updated description",
            Category = "Italian",
            HasDelivery = false
        };

        // Act
        var result = _mapper.Map<Restaurant.Domain.Entities.Restaurant>(command);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Category.Should().Be(command.Category);
        result.HasDelivery.Should().Be(command.HasDelivery);
    }

    [Fact]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        var restaurant = new Restaurant.Domain.Entities.Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "A Test Description",
            Category = "Steakhouse",
            HasDelivery = true,
            ContactEmail = "contact@test.com",
            ContactNumber = "12345",
            Address = new Address { City = "Rome", Street = "Street Name", PostalCode = "00100" },
        };

        // Act
        var result = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(restaurant.Id);
        result.Name.Should().Be(restaurant.Name);
        result.Description.Should().Be(restaurant.Description);
        result.Category.Should().Be(restaurant.Category);
        result.HasDelivery.Should().Be(restaurant.HasDelivery);
        result.City.Should().Be(restaurant.Address.City);
        result.Street.Should().Be(restaurant.Address.Street);
        result.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact]
    public void Should_Map_Restaurant_To_RestaurantDto_When_Address_Is_Null()
    {
        // Arrange
        var restaurant = new Restaurant.Domain.Entities.Restaurant
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "A Test Description",
            Category = "Cafe",
            HasDelivery = false,
            Dishes = new List<Dish>()
        };

        // Act
        var result = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        result.Id.Should().Be(restaurant.Id);
        result.Name.Should().Be(restaurant.Name);
        result.Description.Should().Be(restaurant.Description);
        result.Category.Should().Be(restaurant.Category);
        result.HasDelivery.Should().Be(restaurant.HasDelivery);
        result.City.Should().BeNull();
        result.Street.Should().BeNull();
        result.PostalCode.Should().BeNull();
        result.Dishes.Should().BeEmpty();
    }
}