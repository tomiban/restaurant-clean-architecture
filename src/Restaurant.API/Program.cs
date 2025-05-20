using Restaurant.API.Extensions;
using Restaurant.API.Middlewares;
using Restaurant.Application.Extensions;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Extensions;
using Restaurant.Infrastructure.Seeders;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

// Configura los servicios de la capa de presentación
    builder.AddPresentation();
// Configura los servicios de la capa de aplicación 
    builder.Services.AddApplication();
// Configura los servicios de la capa de infraestructura 
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

// Obtiene un scope para resolver servicios con ámbito (scoped), como el seeder
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

// Seed para la base de datos con datos iniciales si no los hay
    await seeder.Seed();


// Agrega el logging de solicitudes de Serilog para registrar detalles de cada solicitud HTTP entrante
    app.UseSerilogRequestLogging();
// Agrega el middleware de manejo de errores personalizado para capturar excepciones y devolver respuestas de error estandarizadas
    app.UseMiddleware<ErrorHandlingMiddleware>();
// Agrega el middleware personalizado para registrar el tiempo que toma procesar cada solicitud
    app.UseMiddleware<RequestLogTimeMiddleware>();

// Redirige las solicitudes HTTP a HTTPS por seguridad
    app.UseHttpsRedirection();
// Habilita el servicio de archivos estáticos desde wwwroot
    app.UseStaticFiles();
// Habilita las capacidades de enrutamiento para mapear solicitudes a endpoints
    app.UseRouting();

// Configura Swagger/OpenAPI para la documentación de la API y la interfaz de usuario de prueba (solo en Desarrollo)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            options.RoutePrefix = string.Empty;
        });
    }

    app.UsePathBase("/api");
    app.UseRouting();

// Habilita el middleware de autorización para aplicar reglas de control de acceso
    app.UseAuthorization();


// Mapea los endpoints de ASP.NET Core Identity bajo el grupo 'identity'
    app.MapGroup("identity")
        .MapIdentityApi<User>()
        .WithTags("Identity"); // Agrupa estos endpoints bajo la etiqueta 'Auth' en Swagger

// Mapea los controladores basados en atributos (que se encuentran en la carpeta Controllers)
    app.MapControllers();

// --- Logging de Inicio de la Aplicación ---
    var serverAddress = app.Urls.FirstOrDefault() ?? "https://localhost:7285";
    var machineName = Environment.MachineName;
    var environment = app.Environment.EnvironmentName;
    Log.Information("Server running at {ServerUrl} on machine {MachineName} ({Environment})",
        serverAddress, machineName, environment);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error in app startup");
}
finally
{
    Log.CloseAndFlush();
}


public partial class Program
{
    protected Program()
    {
    }
}