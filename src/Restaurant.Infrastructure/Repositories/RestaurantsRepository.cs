using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Constants;
using Restaurant.Domain.Repositories;
using Restaurant.Infrastructure.Persistence;

namespace Restaurant.Infrastructure.Repositories;

public class RestaurantsRepository : IRestaurantsRepository
{
    private static readonly Dictionary<string, Expression<Func<Domain.Entities.Restaurant, object>>> ColumnSelectors =
        new()
        {
            { nameof(Domain.Entities.Restaurant.Name), r => r.Name },
            { nameof(Domain.Entities.Restaurant.Description), r => r.Description },
            { nameof(Domain.Entities.Restaurant.Category), r => r.Category },
        };

    private readonly RestaurantsDbContext _dbContext;

    public RestaurantsRepository(RestaurantsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // CRUD Operations
    public async Task<int> CreateAsync(Domain.Entities.Restaurant restaurant)
    {
        _dbContext.Restaurants.Add(restaurant);
        await _dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task<Domain.Entities.Restaurant?> GetByIdAsync(int id)
    {
        return await _dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Domain.Entities.Restaurant?> UpdateAsync(Domain.Entities.Restaurant restaurant)
    {
        var updatedRestaurant = _dbContext.Restaurants.Update(restaurant);
        await _dbContext.SaveChangesAsync();
        return updatedRestaurant.Entity;
    }

    public async Task DeleteAsync(Domain.Entities.Restaurant restaurant)
    {
        _dbContext.Restaurants.Remove(restaurant);
        await _dbContext.SaveChangesAsync();
    }

    // Collection Retrieval Operations
    public async Task<IEnumerable<Domain.Entities.Restaurant>> GetAllAsync()
    {
        return await _dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
    }

    public async Task<(List<Domain.Entities.Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection? sortDirection)
    {
        var baseQuery = BuildSearchQuery(searchPhrase);
        var totalCount = await baseQuery.CountAsync();

        baseQuery = ApplySorting(baseQuery, sortBy, sortDirection);
        baseQuery = ApplyPaging(baseQuery, pageSize, pageNumber);

        var restaurants = await baseQuery.ToListAsync();
        return (restaurants, totalCount);
    }

    // Utility Operations
    public async Task SaveChanges() => await _dbContext.SaveChangesAsync();

    // Helper Methods
    private IQueryable<Domain.Entities.Restaurant> BuildSearchQuery(string? searchPhrase)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        return _dbContext.Restaurants.Where(r =>
            searchPhraseLower == null ||
            (r.Name.ToLower().Contains(searchPhraseLower) ||
             r.Description.ToLower().Contains(searchPhraseLower)));
    }

    private static IQueryable<Domain.Entities.Restaurant> ApplySorting(
        IQueryable<Domain.Entities.Restaurant> query,
        string? sortBy,
        SortDirection? sortDirection)
    {
        if (sortBy == null || sortDirection == null)
            return query;

        if (!ColumnSelectors.TryGetValue(sortBy, out var selectedColumn))
            return query;

        return sortDirection == SortDirection.Ascending
            ? query.OrderBy(selectedColumn)
            : query.OrderByDescending(selectedColumn);
    }

    private static IQueryable<Domain.Entities.Restaurant> ApplyPaging(
        IQueryable<Domain.Entities.Restaurant> query,
        int pageSize,
        int pageNumber)
    {
        return query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
    }
}