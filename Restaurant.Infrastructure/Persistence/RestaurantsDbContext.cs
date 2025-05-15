using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Infrastructure.Persistence
{
    public class RestaurantsDbContext : IdentityDbContext<User>
    {
        public RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Restaurant>()
                .OwnsOne(r => r.Address); // No crea tabla separada para adress
            modelBuilder.Entity<Domain.Entities.Restaurant>().HasMany(r => r.Dishes).WithOne()
                .HasForeignKey(d => d.RestaurantId);
            modelBuilder.Entity<User>().HasMany(o => o.Restaurants).WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId);
        }
    }
}