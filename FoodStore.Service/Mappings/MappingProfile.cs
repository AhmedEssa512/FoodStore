using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;

namespace FoodStore.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FoodCreateDto, Food>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Food, FoodDto>();

             CreateMap<FoodUpdateDto, Food>()
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)))  
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Description))) 
            .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))  
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())  
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());  // handle ImageUrl and CategoryId separately in the service


        }
    }
}