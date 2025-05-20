using AutoMapper;
using Restaurant.Application.Dishes.Commands.CreateDish;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Dishes.Dtos;

public class DishesProfile : Profile
{
    public DishesProfile()
    {
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>();
    }
}