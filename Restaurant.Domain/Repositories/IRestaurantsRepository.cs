namespace Restaurant.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Entities.Restaurant>> GetAllAsync();
    Task<Entities.Restaurant> GetByIdAsync(int id);
    Task<int> CreateAsync(Entities.Restaurant? restaurant);
    Task<Entities.Restaurant> UpdateAsync(Entities.Restaurant restaurant);
    Task DeleteAsync(Entities.Restaurant? restaurant);

    Task SaveChanges();
}