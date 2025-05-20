using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Restaurant.API.Middlewares;
using Serilog;
using Serilog.Events;

namespace Restaurant.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant API", Version = "v1" });
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    []
                }
            });
            c.UseInlineDefinitionsForEnums();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<RequestLogTimeMiddleware>();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();

        builder.Host.UseSerilog((context, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .WriteTo.Console(
                outputTemplate:
                $"[{{Timestamp:dd-MM HH:mm:ss}} {{Level:u3}}] |{{SourceContext}}{{NewLine}}{{Message:lj}}{{NewLine}}{{Exception}}")); // Registra el logger (Serilog)
    }
}