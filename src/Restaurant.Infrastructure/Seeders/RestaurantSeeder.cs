using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Entities;
using Restaurant.Infrastructure.Persistence;

namespace Restaurant.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext, UserManager<User> userManager) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            // Primero creamos roles si no existen
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }

            // Después creamos el usuario administrador
            var adminId = await EnsureAdminUser();

            // Finalmente creamos restaurantes si no existen
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants(adminId);
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task<string> EnsureAdminUser()
    {
        const string adminEmail = "admin@restaurant.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now).AddYears(-30),
                Nationality = "Argentina"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                await userManager.AddToRoleAsync(adminUser, UserRoles.Owner);
            }
        }

        return adminUser.Id;
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpper() },
            new(UserRoles.User) { NormalizedName = UserRoles.User.ToUpper() },
            new(UserRoles.Owner) { NormalizedName = UserRoles.Owner.ToUpper() }
        ];

        return roles;
    }

    private IEnumerable<Domain.Entities.Restaurant?> GetRestaurants(string ownerId)
    {
        List<Domain.Entities.Restaurant?> restaurants =
        [
            new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description = "Kentucky Fried Chicken - Famous for our Original Recipe chicken",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                ContactNumber = "342432",
                OwnerId = ownerId,

                Dishes =
                [
                    new Dish()
                    {
                        Name = "Original Recipe Bucket",
                        Description = "8 pieces of our famous Original Recipe chicken",
                        Price = 19.99m,
                    },
                    new Dish()
                    {
                        Name = "Zinger Burger",
                        Description = "Spicy chicken burger with lettuce and mayo",
                        Price = 8.99m,
                    }
                ],
                Address = new Address()
                {
                    City = "Madrid",
                    Street = "Gran Vía 28",
                    PostalCode = "28013"
                }
            },
            new()
            {
                Name = "La Paella",
                Category = "Spanish",
                Description = "Authentic Spanish cuisine specializing in paellas and tapas",
                ContactEmail = "info@lapaella.es",
                HasDelivery = true,
                ContactNumber = "911234567",
                OwnerId = ownerId,
                Dishes =
                [
                    new Dish()
                    {
                        Name = "Paella Valenciana",
                        Description = "Traditional rice dish with chicken, rabbit and vegetables",
                        Price = 24.50m,
                    },
                    new Dish()
                    {
                        Name = "Patatas Bravas",
                        Description = "Fried potatoes with spicy tomato sauce",
                        Price = 6.50m,
                    }
                ],
                Address = new Address()
                {
                    City = "Barcelona",
                    Street = "Passeig de Gràcia 43",
                    PostalCode = "08007"
                }
            },
            new()
            {
                Name = "Sushi Master",
                Category = "Japanese",
                Description = "Premium sushi and Japanese specialties",
                ContactEmail = "contact@sushimaster.es",
                HasDelivery = true,
                ContactNumber = "934567890",
                OwnerId = ownerId,
                Dishes =
                [
                    new Dish()
                    {
                        Name = "Dragon Roll",
                        Description = "Eel, cucumber, avocado and special sauce",
                        Price = 15.95m,
                    },
                    new Dish()
                    {
                        Name = "Nigiri Selection",
                        Description = "8 pieces of assorted fresh fish nigiri",
                        Price = 22.00m,
                    }
                ],
                Address = new Address()
                {
                    City = "Valencia",
                    Street = "Calle Colón 12",
                    PostalCode = "46004"
                }
            },
            new()
            {
                Name = "Pizzería Roma",
                Category = "Italian",
                Description = "Authentic Italian pizzas baked in wood-fired oven",
                ContactEmail = "info@pizzeriaroma.es",
                HasDelivery = true,
                ContactNumber = "917654321",
                OwnerId = ownerId,
                Dishes =
                [
                    new Dish()
                    {
                        Name = "Margherita",
                        Description = "Tomato sauce, mozzarella, and fresh basil",
                        Price = 12.50m,
                    },
                    new Dish()
                    {
                        Name = "Quattro Formaggi",
                        Description = "Four cheese pizza with mozzarella, gorgonzola, parmesan, and goat cheese",
                        Price = 16.95m,
                    }
                ],
                Address = new Address()
                {
                    City = "Sevilla",
                    Street = "Avenida de la Constitución 20",
                    PostalCode = "41004"
                }
            }
        ];

        return restaurants;
    }
}