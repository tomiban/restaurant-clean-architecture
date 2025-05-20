using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurant.Application.Users;
using Restaurant.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_withAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, "Admin"),
            new(ClaimTypes.Role, "User"),
            new("Nationality", "Argentina"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));

        httpContextAccesorMock.Setup(x => x.HttpContext).Returns(
            new DefaultHttpContext() { User = user });

        var userContext = new UserContext(httpContextAccesorMock.Object);

        //act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Nationality.Should().Be("Argentina");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
    }

    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        httpContextAccesorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccesorMock.Object);

        // Act
        Action action = () => userContext.GetCurrentUser();
        // Assert

        action.Should().Throw<InvalidOperationException>().WithMessage("User context not found");
    }
}