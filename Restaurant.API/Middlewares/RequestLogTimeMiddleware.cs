using System.Diagnostics;

namespace Restaurant.API.Middlewares;

public class RequestLogTimeMiddleware(ILogger<RequestLogTimeMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var timer = Stopwatch.StartNew();
        await next.Invoke(context);
        timer.Stop();

        if (timer.ElapsedMilliseconds / 1000 > 4)
            logger.LogWarning("Request [{Verb}] at {Path} took {Time} ms", context.Request.Method, context.Request.Path,
                timer.ElapsedMilliseconds);
    }
}