using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;

namespace FoodStore.Service.Mappings
{
    public class OrderProfile : Profile
    {
         public OrderProfile()
        {
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => new List<OrderDetail>())) 
                .ForMember(dest => dest.Total, opt => opt.Ignore()); // Optionally ignore Total, since it's being set later

            // CreateMap<Order, OrderResponseDto>();

            CreateMap<Order, OrderResponseDto>()
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                 .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

        // Map OrderDetail entity to OrderDetailsDto
            CreateMap<OrderDetail, OrderDetailsDto>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.Name)) // Mapping Food name to FoodName
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Food.Price)); 
       }

        
    }
}