using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Constants;
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


    public async Task<(List<Domain.Entities.Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase, int? pageSize, int? pageNumber, string? sortBy,
        SortDirection? sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = dbContext.Restaurants.Where(r =>
            searchPhraseLower == null ||
            (r.Name.ToLower().Contains(searchPhraseLower) ||
             r.Description.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null && sortDirection != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Domain.Entities.Restaurant, object>>>()
            {
                { nameof(Domain.Entities.Restaurant.Name), r => r.Name },
                { nameof(Domain.Entities.Restaurant.Description), r => r.Description },
                { nameof(Domain.Entities.Restaurant.Category), r => r.Category },
            };
            if (columnsSelector.TryGetValue(sortBy, out var selectedColumn))
            {
                baseQuery = sortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }
        }

        if (pageSize.HasValue && pageNumber.HasValue && pageSize.Value > 0 && pageNumber.Value > 0)
        {
            baseQuery = baseQuery
                .Skip(pageSize.Value * (pageNumber.Value - 1))
                .Take(pageSize.Value);
        }

        var restaurants = await baseQuery.ToListAsync();

        return (restaurants, totalCount);
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