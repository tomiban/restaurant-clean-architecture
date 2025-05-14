using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Application.Restaurants.Queries.GetRestaurantById;
using Restaurant.Domain.Constants;
using Restaurant.Infrastructure.Authorization;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("restaurants")]
[Authorize]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = PolicyNames.IsOfLegalAge)]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand restaurant)
    {
        int id = await mediator.Send(restaurant);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto>> Update([FromRoute] int id,
        [FromBody] UpdateRestaurantCommand restaurant)
    {
        restaurant.Id = id;
        var updatedRestaurant = await mediator.Send(restaurant);
        return Ok(updatedRestaurant);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
}