## Infraestructura

En este proyecto, maneja principalmente:

### Componentes principales

1. **RestaurantsDbContext**: Contexto de Entity Framework Core

    - Define los DbSets para las entidades (Restaurant, Dish)
    - Configura las relaciones entre entidades
    - Implementa owned types para evitar tablas adicionales (Address)

2. **Extensiones de Servicios**

    - `ServiceCollectionExtensions`: Clase estática que proporciona métodos de extensión para registrar los servicios de
      infraestructura
    - `AddInfrastructure`: Configura DbContext con PostgreSQL y registra seeders

3. **Seeders**
    - `RestaurantSeeder`: Inicializa la base de datos con datos de prueba
    - Implementa la interfaz `IRestaurantSeeder`
    - Verifica la conexión y si no hay registros, inserta datos de ejemplo

### Decisiones de diseño

- **Configuración de relaciones**: Se utilizó Fluent API para definir relaciones y configurar owned types
- **Connection string**: Se obtiene de la configuración y se valida su existencia
- **Seeders**: Implementados como servicios que pueden inyectarse y ejecutarse durante el inicio de la aplicación

### Ventajas de esta implementación

- Separación clara de responsabilidades
- Facilidad para cambiar de proveedor de base de datos
- Inyección de dependencias para facilitar pruebas
- Inicialización automática de datos para desarrollo

Capa para configurar contexto de entitiy frameworks y migraciones a base de datos

1. Crea una clase estatica AddInfraestructureService para poder inyectar los servicios en

2. Crea un seeder para poder inicializar la base de datos con datos de prueba

3. Crea un contexto de entitiy frameworks para poder realizar las migraciones a la base de datos

4. Crea un seeder para poder inicializar la base de datos con datos de prueba
