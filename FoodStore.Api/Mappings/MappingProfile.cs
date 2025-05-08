using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;

namespace FoodStore.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FoodDto, Food>()
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}