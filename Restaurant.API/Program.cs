using Restaurant.API.Extensions;
using Restaurant.API.Middlewares;
using Restaurant.Application.Extensions;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Extensions;
using Restaurant.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de Servicios ---
// Configura los servicios de la capa de presentación (Controladores, Swagger, Logging, Middlewares)
builder.AddPresentation();
// Configura los servicios de la capa de aplicación (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();
// Configura los servicios de la capa de infraestructura (DbContext, Identity, Repositorios, Seeder)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// --- Seed de Base de Datos ---
// Obtiene un scope para resolver servicios con ámbito (scoped), como el seeder
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
// Siembra la base de datos con datos iniciales si es necesario
await seeder.Seed();

// --- Configuración del Pipeline de Middleware ---
// Agrega el logging de solicitudes de Serilog para registrar detalles de cada solicitud HTTP entrante
app.UseSerilogRequestLogging();
// Agrega el middleware de manejo de errores personalizado para capturar excepciones y devolver respuestas de error estandarizadas
app.UseMiddleware<ErrorHandlingMiddleware>();
// Agrega el middleware personalizado para registrar el tiempo que toma procesar cada solicitud
app.UseMiddleware<RequestLogTimeMiddleware>();

// Redirige las solicitudes HTTP a HTTPS por seguridad
app.UseHttpsRedirection();
// Habilita el servicio de archivos estáticos (como CSS, JavaScript, imágenes) desde wwwroot
app.UseStaticFiles();
// Habilita las capacidades de enrutamiento para mapear solicitudes a endpoints
app.UseRouting();

// Configura Swagger/OpenAPI para la documentación de la API y la interfaz de usuario de prueba (solo en Desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Genera el archivo swagger.json
    app.UseSwaggerUI(options => // Sirve la interfaz de usuario de Swagger
    {
        // Apunta la interfaz de usuario al archivo swagger.json generado
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        // Sirve la interfaz de usuario de Swagger en la raíz de la aplicación
        options.RoutePrefix = string.Empty;
    });
}

// Establece una ruta base para los endpoints de la API (p. ej., todos los endpoints estarán bajo /api)
// Considera eliminarlo si no es estrictamente necesario o si causa problemas de enrutamiento.
app.UsePathBase("/api");
// Vuelve a ejecutar el enrutamiento después de cambiar la ruta base. Esto también podría ser redundante según las necesidades exactas.
app.UseRouting();

// Habilita el middleware de autorización para aplicar reglas de control de acceso
app.UseAuthorization();

// --- Mapeo de Endpoints ---
// Mapea los endpoints de ASP.NET Core Identity (login, registro, etc.) bajo el grupo 'identity'
app.MapGroup("identity")
    .MapIdentityApi<User>()
    .WithTags("Identity"); // Agrupa estos endpoints bajo la etiqueta 'Auth' en Swagger

// Mapea los controladores basados en atributos (que se encuentran en la carpeta Controllers)
app.MapControllers();

// --- Logging de Inicio de la Aplicación ---
// Registra la información del servidor al inicio
var serverAddress = app.Urls.FirstOrDefault() ?? "https://localhost:7285";
var machineName = Environment.MachineName;
var environment = app.Environment.EnvironmentName;
Log.Information("Server running at {ServerUrl} on machine {MachineName} ({Environment})",
    serverAddress, machineName, environment);

app.Run();