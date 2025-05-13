using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> CreateAsync(Dish dish);
    Task<Dish> UpdateAsync(Dish dish);
    Task DeleteAsync(Dish dish);
    Task SaveChanges();
}