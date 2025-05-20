using Restaurant.Domain.Constants;

namespace Restaurant.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Entities.Restaurant>> GetAllAsync();

    Task<(List<Entities.Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection? sortDirection
    );

    Task<Entities.Restaurant> GetByIdAsync(int id);
    Task<int> CreateAsync(Entities.Restaurant? restaurant);
    Task<Entities.Restaurant> UpdateAsync(Entities.Restaurant restaurant);
    Task DeleteAsync(Entities.Restaurant? restaurant);

    Task SaveChanges();
}