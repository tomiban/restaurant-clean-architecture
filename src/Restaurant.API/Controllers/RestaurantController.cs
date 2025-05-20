using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurant.Application.Restaurants.Dtos;
using Restaurant.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurant.Application.Restaurants.Queries.GetRestaurantById;
using Restaurant.Domain.Constants;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("restaurants")]
[Authorize]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
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

    [HttpPost("{id}/logo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var command = new UploadRestaurantLogoCommand()
        {
            RestaurantId = id,
            FileName = file.FileName,
            File = stream
        };
        await mediator.Send(command);
        return NoContent();
    }
}