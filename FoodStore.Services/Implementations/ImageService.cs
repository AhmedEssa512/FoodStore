using FoodStore.Contracts.Interfaces;
using FoodStore.Services.Exceptions;
using Microsoft.AspNetCore.Http;


namespace FoodStore.Services.Implementations
{
    public class ImageService: IImageService
    {
         private readonly IHttpContextAccessor _httpContextAccessor;
         public ImageService(IHttpContextAccessor httpContextAccessor)
         {
            _httpContextAccessor = httpContextAccessor;
         }

        public string GetFullUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return string.Empty;

            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null)
                return relativePath; 

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{relativePath}";
        }

        public async Task<string> SaveImageAsync(Stream imageStream, string originalFileName)
        {
            if (imageStream == null || imageStream.Length == 0)
            {
                throw new BadRequestException("Invalid image stream.");
            }

            string imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            string imagePath = Path.Combine(imagesFolder, imageFileName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageStream.CopyToAsync(fileStream);
            }

            return Path.Combine("images", imageFileName).Replace("\\", "/"); // For web URL compatibility
        }

        public async Task<string> ReplaceImageAsync(Stream imageStream, string originalFileName, string? oldImageUrl)
        {
            var newImageUrl = await SaveImageAsync(imageStream, originalFileName);

            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                await DeleteImageAsync(oldImageUrl);
            }

            return newImageUrl;
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