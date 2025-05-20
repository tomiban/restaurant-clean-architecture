using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Dishes.Commands.CreateDish;
using Restaurant.Application.Dishes.Commands.DeleteDish;
using Restaurant.Application.Dishes.Dtos;
using Restaurant.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurant.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurant.Domain.Entities;

namespace Restaurant.API.Controllers
{
    [Route("restaurant/{restaurantId}/dishes")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishesForRestaurant([FromRoute] int restaurantId)
        {
            var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);
        }


        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetAllDishesForRestaurant([FromRoute] int restaurantId,
            [FromRoute] int dishId)
        {
            var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            return Ok(dish);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand dish)
        {
            dish.RestaurantId = restaurantId;
            int Id = await mediator.Send(dish);
            return CreatedAtAction("GetDish", new { id = Id }, dish);
        }

        [HttpDelete("{dishId}")]
        public async Task<IActionResult> DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            await mediator.Send(new DeleteDishCommand(restaurantId, dishId));
            return NoContent();
        }
    }
}