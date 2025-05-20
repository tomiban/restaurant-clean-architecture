Aquí tienes el apunte completo e integrado, incluyendo toda la información detallada sobre la capa de autorización en tu
aplicación:

---

# 🛡️ Entendiendo la capa de autorización en tu aplicación (Apunte completo)

## 1. Mecanismos de autorización en tu aplicación

Tu aplicación implementa tres mecanismos distintos pero complementarios:

1. **Role-Based Authorization** (basada en roles)
2. **Policy-Based Authorization** (basada en políticas)
    * Políticas simples basadas en Claims
    * Políticas avanzadas basadas en Requirements
3. **Resource-Based Authorization** (basada en recursos)

Cada mecanismo cumple un propósito distinto y trabajan en conjunto para formar una capa de seguridad robusta.

---

## 2. Role-Based Authorization

Este mecanismo se basa en roles asignados a los usuarios como "Admin", "User", etc.

### Configuración:

```csharp
services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
    .AddEntityFrameworkStores<RestaurantsDbContext>();
```

### Uso típico:

```csharp
[Authorize(Roles = "Admin")]
public IActionResult AdminAction() { ... }
```

### También se usa dentro de `RestaurantAuthorizationService`:

```csharp
if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
{
    return true;
}
```

Esto permite lógica condicional según el rol del usuario.

---

## 3. Policy-Based Authorization

### 3.1 Políticas simples basadas en Claims

Estas verifican si el usuario tiene un cierto claim:

```csharp
.AddPolicy(PolicyNames.HasNationality,
    builder => builder.RequireClaim(ClaimTypes.Nationality, ["Argentina"]))
```

* No requieren lógica personalizada.
* Los claims como nacionalidad y fecha de nacimiento se agregan mediante `RestaurantsUserClaimsPrincipalFactory`.

### 3.2 Políticas avanzadas basadas en Requirements

Estas requieren lógica personalizada para validar una condición compleja:

```csharp
.AddPolicy(PolicyNames.IsOfLegalAge, 
    builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
.AddPolicy(PolicyNames.MinimumRestaurantCreated, 
    builder => builder.AddRequirements(new CreatedMultipleRequirement(1)))
```

#### Cada Requirement implica:

1. Clase que implementa `IAuthorizationRequirement`
2. Handler que hereda de `AuthorizationHandler<TRequirement>`
3. Lógica personalizada de autorización

#### Ejemplo:

* `MinimumAgeRequirementHandler` calcula la edad a partir de la fecha de nacimiento.
* `CreatedMultipleRequirementHandler` verifica cuántos restaurantes ha creado el usuario.

---

## 4. Resource-Based Authorization

Se implementa con `RestaurantAuthorizationService`.

### Lógica:

```csharp
public bool Authorize(Domain.Entities.Restaurant restaurant, ResourceOperation operation)
{
    // Verifica si el usuario tiene permiso sobre este recurso
}
```

* Depende de la relación entre el usuario y el recurso (dueño, admin, etc.)
* Se utiliza dentro de la lógica de negocio, no en los controllers.

### Ejemplo:

```csharp
if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
    throw new ForbidException();
```

---

## 5. Cómo trabajan juntos estos mecanismos

### Flujo típico:

1. **Nivel controller**: Validaciones generales con `[Authorize(...)]`

   ```csharp
   [Authorize(Policy = "IsOfLegalAge")]
   public class RestaurantController { ... }
   ```

2. **Nivel handler (negocio)**: Validaciones sobre el recurso

   ```csharp
   if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
       throw new ForbidException();
   ```

---

## 6. Diferencias clave

| Aspecto              | Role-Based | Claim Policy | Requirement Policy | Resource-Based |
|----------------------|------------|--------------|--------------------|----------------|
| Lógica simple        | ✅          | ✅            | ❌                  | ❌              |
| Lógica personalizada | ❌          | ❌            | ✅                  | ✅              |
| Requiere Handler     | ❌          | ❌            | ✅                  | ✅              |
| Verifica recurso     | ❌          | ❌            | ❌                  | ✅              |
| Se usa en controller | ✅          | ✅            | ✅                  | ❌              |
| Se usa en negocio    | ❌          | ❌            | ✅ (a veces)        | ✅              |

---

## 7. Cuándo usar cada mecanismo

* **Role-Based**: Para restricciones amplias por función.

  > Ej: "Solo admins pueden gestionar usuarios."

* **Policy con Claims**: Para verificaciones simples de datos del usuario.

  > Ej: "Solo usuarios de Argentina pueden acceder."

* **Policy con Requirements**: Para reglas complejas y cálculos.

  > Ej: "Solo mayores de 20 años", "Solo quienes crearon al menos 1 restaurante."

* **Resource-Based**: Para reglas dependientes del recurso específico.

  > Ej: "Solo el dueño del restaurante puede editarlo."

---

## 8. Configuración y registro

### Identity + Roles + Claims:

```csharp
services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
    .AddEntityFrameworkStores<RestaurantsDbContext>();
```

### Registro de handlers:

```csharp
services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
services.AddScoped<IAuthorizationHandler, CreatedMultipleRequirementHandler>();
services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
```

### Definición de políticas:

```csharp
services.AddAuthorizationBuilder()
    .AddPolicy(PolicyNames.HasNationality, builder =>
        builder.RequireClaim(ClaimTypes.Nationality, ["Argentina"]))
    .AddPolicy(PolicyNames.IsOfLegalAge, builder =>
        builder.AddRequirements(new MinimumAgeRequirement(20)))
    .AddPolicy(PolicyNames.MinimumRestaurantCreated, builder =>
        builder.AddRequirements(new CreatedMultipleRequirement(1)));
```

---

## 9. Conclusión

Tu sistema de autorización está bien estructurado en capas:

✅ Roles → para funciones amplias
✅ Claims → para condiciones simples
✅ Requirements → para reglas avanzadas
✅ Servicios → para permisos sobre recursos específicos

Esto te permite tener:

* Seguridad declarativa en controllers
* Validación personalizada en la lógica de negocio
* Máxima flexibilidad para expandir

