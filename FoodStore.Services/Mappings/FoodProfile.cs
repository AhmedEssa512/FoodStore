using AutoMapper;
using FoodStore.Data.Entities;
using FoodStore.Contracts.DTOs.Food;
using FoodStore.Services.Mappings.Resolvers;

namespace FoodStore.Services.Mappings
{
    public class FoodProfile : Profile
    {
        public FoodProfile()
        {
            CreateMap<FoodCreateDto, Food>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Food, FoodResponseDto>()
             .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom<ImageUrlResolver>());


             CreateMap<FoodUpdateDto, Food>()
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)))  
            .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Description))) 
            .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))  
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())  
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());  // handle ImageUrl and CategoryId separately in the service

            CreateMap<Food, FoodAdminListDto>()
            .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                    .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom<ImageUrlResolver>());
        }
    }
}