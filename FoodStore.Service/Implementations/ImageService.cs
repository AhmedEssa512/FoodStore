using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Exceptions;
using Microsoft.AspNetCore.Http;

namespace FoodStore.Service.Implementations
{
    public class ImageService: IImageService
    {
        public async Task<string> SaveImageAsync(IFormFile image)
        {

            string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFileName);

            var imageDirectory = Path.GetDirectoryName(imagePath);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"images/{imageFileName}";
        }

        public async Task DeleteImageAsync(string imagePath)
        {
           if(! string.IsNullOrWhiteSpace(imagePath))
           {

                string fullPath =  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                if (File.Exists(fullPath))
                {    
                    await Task.Run(() => File.Delete(fullPath));            
                }
                else
                {
                    throw new NotFoundException($"File not found: {imagePath}");
                }
           }
        
        }
    }
}