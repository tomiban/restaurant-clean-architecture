using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurant.API.Middlewares;
using Restaurant.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

[TestSubject(typeof(ErrorHandlingMiddleware))]
public class ErrorHandlingMiddlewareTest
{
    private readonly DefaultHttpContext _context;
    private readonly Mock<IHostEnvironment> _hostEnvironmentMock;
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _middleware;

    public ErrorHandlingMiddlewareTest()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _hostEnvironmentMock = new Mock<IHostEnvironment>();
        _middleware = new ErrorHandlingMiddleware(_loggerMock.Object, _hostEnvironmentMock.Object);
        _context = new DefaultHttpContext();
    }

    [Fact]
    public async Task InvokeAsync_WhenNotExceptionThrown_ShouldCallNextDelegate()
    {
        var nextDelegateCalled = new Mock<RequestDelegate>();
        // Act
        await _middleware.InvokeAsync(_context, nextDelegateCalled.Object);

        // Assert
        nextDelegateCalled.Verify(x => x.Invoke(_context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        var notFoundException = new NotFoundException(nameof(Restaurant.Domain.Entities.Restaurant), "Not Found");
        // Act
        await _middleware.InvokeAsync(_context, _ => throw notFoundException);

        // Assert
        _context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        var forbidException = new ForbidException();
        // Act
        await _middleware.InvokeAsync(_context, _ => throw forbidException);

        // Assert
        _context.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        var exception = new Exception();
        // Act
        await _middleware.InvokeAsync(_context, _ => throw exception);

        // Assert
        _context.Response.StatusCode.Should().Be(500);
    }
}