using AutoMapper;
using Restaurant.Application.Restaurants.Commands.CreateRestaurant;
using Restaurant.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        // Create
        CreateMap<CreateRestaurantCommand, Domain.Entities.Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => new Address()
                {
                    City = src.City,
                    PostalCode = src.PostalCode,
                    Street = src.Street,
                }
            ));

        // Update
        CreateMap<UpdateRestaurantCommand, Domain.Entities.Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(src => new Address()
                {
                    City = src.City,
                    PostalCode = src.PostalCode,
                    Street = src.Street,
                }
            ));

        // Get   
        CreateMap<Domain.Entities.Restaurant, RestaurantDto>()
            .ForMember(d => d.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(d => d.PostalCode, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(d => d.Street, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(d => d.Dishes, opt =>
                opt.MapFrom(src => src.Dishes));
    }
}