using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Users.Commands.AssignUserRole;
using Restaurant.Application.Users.Commands.UnassingUserRole;
using Restaurant.Application.Users.Commands.UpdateUserDetails;
using Restaurant.Application.Users.Dtos;
using Restaurant.Application.Users.Queries.GetUserInfo;
using Restaurant.Domain.Constants;

namespace Restaurant.API.Controllers;

[Route("identity")]
[Tags("Identity")]
[ApiController]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPut("user")]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("user/details")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        var result = await mediator.Send(new GetUserInfoQuery());
        return Ok(result);
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UnassignUserRole(UnassingUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}