

namespace FoodStore.Contracts.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(Stream imageStream, string originalFileName);
        Task DeleteImageAsync(string imagePath);
        Task<string> ReplaceImageAsync(Stream imageStream, string originalFileName, string? oldImageUrl);
        public string GetFullUrl(string relativePath);
    }
}