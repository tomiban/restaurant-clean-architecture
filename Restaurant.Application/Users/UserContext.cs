using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Restaurant.Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null) throw new InvalidOperationException("User context not found");
        if (user.Identity is null || user.Identity.IsAuthenticated is false) return null;
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
        var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;
        var dateOfBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var dateOfBirth = dateOfBirthString is null
            ? (DateOnly?)null
            : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");
        return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
    }
}