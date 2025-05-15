using Restaurant.Domain.Exceptions;

namespace Restaurant.API.Middlewares;

public class ErrorHandlingMiddleware(
    ILogger<ErrorHandlingMiddleware> logger,
    IHostEnvironment env
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { Message = notFound.Message });
            logger.LogWarning(notFound.Message);
        }

        catch (UnauthorizedException unauthorized)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { Message = unauthorized.Message });
            logger.LogWarning(unauthorized.Message);
        }

        catch (ForbidException forbid)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { Message = "Access forbbiden" });
            logger.LogWarning(forbid.Message);
        }
        catch (Exception ex)
        {
            // En desarrollo
            var response = env.IsDevelopment()
                ? new { Message = ex.Message, StackTrace = ex.StackTrace }
                : new { Message = "Something went wrong", StackTrace = (string?)null };

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(response);
            logger.LogError(ex, ex.Message);
        }
    }
}