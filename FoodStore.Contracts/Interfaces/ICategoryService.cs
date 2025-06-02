using FoodStore.Contracts.DTOs.Category;


namespace FoodStore.Contracts.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task DeleteCategoryAsync(int categoryId);
        Task UpdateCategoryAsync(int categoryId,CategoryDto categoryDto);
        Task<IReadOnlyList<CategoryResponseDto>> GetCategoriesAsync();
        Task<CategoryResponseDto> GetCategoryByIdAsync(int categoryId);

    }
}