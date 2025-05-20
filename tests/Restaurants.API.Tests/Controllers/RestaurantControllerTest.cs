using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurant.API.Controllers;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Repositories;
using Xunit;

namespace Restaurants.API.Tests.Controllers;

[TestSubject(typeof(RestaurantController))]
public class RestaurantControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                    _ => _restaurantsRepositoryMock.Object));
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange  
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetAllMatchingAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<string>(), It.IsAny<SortDirection?>()))
            .ReturnsAsync((new List<Restaurant.Domain.Entities.Restaurant>(), 0));

        var request = "/api/restaurants?PageNumber=1&PageSize=10";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
    {
        // Arrange  
        var request = "/api/restaurants";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_Returns404NotFound()
    {
        // Arrange
        var id = 9999;
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Restaurant.Domain.Entities.Restaurant?)null);

        var request = $"/api/restaurants/{id}";

        // Act
        var response = await _client.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForExistingId_Returns200Ok()
    {
        // Arrange
        var id = 1;
        var restaurant = new Restaurant.Domain.Entities.Restaurant()
        {
            Id = id,
            Name = "Test",
            Description = "Test",
            Category = "Italian",
        };
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetByIdAsync(restaurant.Id))
            .ReturnsAsync(restaurant);

        var request = $"/api/restaurants/{id}";

        // Act
        var response = await _client.GetAsync(request);
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test");
        restaurantDto.Category.Should().Be("Italian");
    }
}