using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Persistence;

namespace Restaurant.Infrastructure.Repositories;

public class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> CreateAsync(Dish dish)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
        return dish.Id;
    }

    public Task<Dish> UpdateAsync(Dish dish)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Dish dish)
    {
        dbContext.Dishes.Remove(dish);
        await SaveChanges();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}