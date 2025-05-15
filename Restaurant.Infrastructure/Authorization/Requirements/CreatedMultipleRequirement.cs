using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRequirement(int minimumCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; set; } = minimumCreated;
}