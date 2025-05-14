using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FoodStore.Service.Abstracts
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile image);
        Task DeleteImageAsync(string imagePath);
    }
}