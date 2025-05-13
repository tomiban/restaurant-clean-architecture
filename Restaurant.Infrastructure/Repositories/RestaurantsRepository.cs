using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Persistence;

namespace Restaurant.Infrastructure.Repositories;

public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Domain.Entities.Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return restaurants;
    }

    public async Task<Domain.Entities.Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
        return restaurant;
    }

    public async Task<int> CreateAsync(Domain.Entities.Restaurant? restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task<Domain.Entities.Restaurant?> UpdateAsync(Domain.Entities.Restaurant? restaurant)
    {
        var updatedRestaurant = dbContext.Restaurants.Update(restaurant);
        await dbContext.SaveChangesAsync();
        return updatedRestaurant.Entity;
    }

    public async Task DeleteAsync(Domain.Entities.Restaurant? restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges() => await dbContext.SaveChangesAsync();
}