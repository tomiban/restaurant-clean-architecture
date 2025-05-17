using FluentAssertions;
using Restaurant.Application.Users;
using Restaurant.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests;

public class CurrentUserTests
{
    // Naming convention -> TestMethod_Scenario_ExpectedResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRoleTest_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // arange  -> Setup del objecto
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.Owner, UserRoles.User],
            null, null);

        // act -> Obtener el valor de la propiedad

        var isInRole = currentUser.IsInRole(roleName);

        // assert -> Verificar el resultado
        isInRole.Should().BeTrue();
    }


    [Fact]
    public void IsInRoleTest_WithNoMatchingRole_ShouldReturnFalse()
    {
        // arange  -> Setup del objecto
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act -> Obtener el valor de la propiedad

        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // assert -> Verificar el resultado
        isInRole.Should().BeFalse();
    }

    [Fact]
    public void IsInRoleTest_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // arange  -> Setup del objecto
        var currentUser = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act -> Obtener el valor de la propiedad

        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // assert -> Verificar el resultado
        isInRole.Should().BeFalse();
    }
}