# Sistema de Gestión de Restaurantes

API desarrollada con .NET que proporciona una solución completa para la gestión de restaurantes.

## Arquitectura

El proyecto sigue los principios de la Arquitectura Limpia (Clean Architecture) con una estructura en capas:

- **Restaurant.API**: Capa de presentación - Controllers, Middlewares y configuración de la API
- **Restaurant.Application**: Capa de aplicación - Lógica de negocio, DTOs y servicios
- **Restaurant.Domain**: Capa de dominio - Entidades, interfaces y reglas de negocio
- **Restaurant.Infrastructure**: Capa de infraestructura - Implementaciones de persistencia y servicios externos

El proyecto implementa el patrón CQRS (Command Query Responsibility Segregation) para separar las operaciones de lectura y escritura.

## Tecnologías

- ASP.NET Core
- Entity Framework Core
- MediatR para implementación de CQRS
- Swagger/OpenAPI
- Serilog para logging
- Docker y Docker Compose

## Características

- Estructura modular siguiendo Clean Architecture
- Patrón CQRS con MediatR para separación de responsabilidades
- API RESTful documentada con Swagger
- Autenticación y autorización con ASP.NET Core Identity
- Manejo global de errores
- Logging de solicitudes y tiempos de respuesta
- Seeders para datos iniciales
- Soporte para contenedores Docker

## Configuración

### Credenciales

Por razones de seguridad, las credenciales no se incluyen en el repositorio. Debes configurar:

1. Las cadenas de conexión a la base de datos
2. Las claves de acceso a Azure Blob Storage

Edita los archivos `appsettings.json` y `appsettings.Development.json` con tus credenciales:

```json
{
  "ConnectionStrings": {
    "RestaurantsDb": "User Id=TU_USUARIO;Password=TU_CONTRASEÑA;Server=TU_SERVIDOR;Port=5432;Database=postgres"
  },
  "BlobStorage": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=TU_CUENTA;AccountKey=TU_CLAVE;EndpointSuffix=core.windows.net",
    "LogosContainerName": "logos",
    "AccountKey": "TU_CLAVE"
  }
}
```

## Inicio Rápido

### Prerequisitos

- .NET 7.0 o superior
- Docker (opcional)

### Ejecución con Docker

```bash
docker-compose up
```

### Ejecución local

```bash
cd src/Restaurant.API
dotnet run
```

La API estará disponible en: https://localhost:7285

La documentación Swagger estará disponible en la ruta raíz.

## Pruebas

El proyecto incluye tests en las siguientes capas:

- Restaurants.API.Tests
- Restaurants.Application.Tests
- Restaurants.Infrastructure.Tests

Las pruebas están implementadas utilizando xUnit como framework de testing.

Para ejecutar las pruebas:

```bash
dotnet test
```

## Estructura de Directorios

```
├── src/
│   ├── Restaurant.API/         # Capa de presentación
│   ├── Restaurant.Application/ # Capa de aplicación
│   ├── Restaurant.Domain/      # Capa de dominio
│   └── Restaurant.Infrastructure/ # Capa de infraestructura
├── tests/                      # Tests unitarios y de integración con xUnit
├── compose.yaml                # Configuración de Docker Compose
└── README.md                   # Este archivo
```
