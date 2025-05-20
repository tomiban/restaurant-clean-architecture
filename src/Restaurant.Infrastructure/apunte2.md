# Arquitectura

El proyecto sigue una arquitectura de Capas con Clean Architecture:

1. **Domain Layer (Restaurant.Domain)**

    - Entidades: Restaurant, User, Dish, Address
    - Interfaces y repositorios
    - Excepciones y constantes del dominio

2. **Application Layer (Restaurant.Application)**

    - CQRS con MediatR (Commands/Queries)
    - DTOs para transferencia de datos
    - Validación y lógica de negocio

3. **Infrastructure Layer (Restaurant.Infrastructure)**

    - Implementación de repositorios
    - Configuración de autorización
    - Persistencia (EF Core) y migraciones
    - Seeders para datos iniciales

4. **API Layer (Restaurant.API)**
    - Controllers REST
    - Middlewares (Error handling, Request logging)
    - Configuración de la aplicación

## Flujo de la aplicación

1. Las solicitudes HTTP llegan a los Controllers en la capa API
2. Los controllers usan MediatR para enviar Commands/Queries
3. Los handlers en Application procesan las solicitudes:
    - Commands modifican datos (Create/Update/Delete)
    - Queries obtienen datos (GetAll/GetById)
4. Los handlers utilizan repositorios para acceder a la capa de datos
5. La autorización valida permisos en varios niveles

## Sistema de autorización

Implementa tres mecanismos complementarios:

1. **Role-Based Authorization**

    - Roles como "Admin" y "Owner"
    - Decoradores en controllers: `[Authorize(Roles = "Admin")]`

2. **Policy-Based Authorization**

    - Políticas simples basadas en Claims (nacionalidad)
    - Políticas complejas con Requirements (edad mínima)

3. **Resource-Based Authorization**
    - Validación a nivel de recursos específicos
    - Ejemplo: Solo el propietario puede editar su restaurante

## Creación de nuevas features

Para añadir una nueva funcionalidad:

1. Definir entidades en Domain si son necesarias
2. Crear Commands/Queries en Application:
    - Command/Query con datos de entrada
    - Handler que procesa la solicitud
    - DTOs para respuestas
3. Implementar repositorios en Infrastructure
4. Añadir endpoints en Controllers API
5. Configurar autorización según necesidades

## Patrones implementados

- Clean Architecture
- CQRS con MediatR
- Repository Pattern
- Dependency Injection
- Exception Middleware Handling
- Identity para autenticación

El proyecto está bien estructurado para escalabilidad y mantenimiento, con una clara separación de responsabilidades
entre capas.
