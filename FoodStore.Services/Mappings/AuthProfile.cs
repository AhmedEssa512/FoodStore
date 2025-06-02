using AutoMapper;
using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.DTOs.Auth;
using FoodStore.Services.Models;

namespace FoodStore.Services.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AuthResult, AuthResponseBase>()
            .ForMember(dest => dest.IsAuthenticated, opt => opt.MapFrom(src => src.IsAuthenticated))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));

            CreateMap<AuthResult, LoginResponseDto>().IncludeBase<AuthResult, AuthResponseBase>();
            CreateMap<AuthResult, RegisterResponseDto>().IncludeBase<AuthResult, AuthResponseBase>();
            CreateMap<AuthResult, RefreshTokenResponseDto>().IncludeBase<AuthResult, AuthResponseBase>();

        }
    }
}