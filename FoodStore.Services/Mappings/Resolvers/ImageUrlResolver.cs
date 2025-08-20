using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodStore.Contracts.DTOs.Food;
using FoodStore.Contracts.Interfaces;
using FoodStore.Data.Entities;

namespace FoodStore.Services.Mappings.Resolvers
{
    public class ImageUrlResolver : IValueResolver<Food, object, string>
    {
        private readonly IImageService _imageService;
        public ImageUrlResolver(IImageService imageService)
        {
            _imageService = imageService;
        }
        public string Resolve(Food source, object destination, string destMember, ResolutionContext context)
        {
            return _imageService.GetFullUrl(source.ImageUrl);
        }
    }
}