using FoodStore.Contracts.Interfaces;
using FoodStore.Services.Exceptions;


namespace FoodStore.Services.Implementations
{
    public class ImageService: IImageService
    {
        // public async Task<string> SaveImageAsync(IFormFile image)
        // {

        //     string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        //     var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFileName);

        //     var imageDirectory = Path.GetDirectoryName(imagePath);
        //     if (!Directory.Exists(imageDirectory))
        //     {
        //         Directory.CreateDirectory(imageDirectory!);
        //     }

        //     using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //     {
        //         await image.CopyToAsync(fileStream);
        //     }

        //     return $"images/{imageFileName}";
        // }

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