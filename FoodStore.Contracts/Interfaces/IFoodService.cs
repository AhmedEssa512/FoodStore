using FoodStore.Contracts.DTOs.Food;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.QueryParams;

namespace FoodStore.Contracts.Interfaces
{
    public interface IFoodService 
    {
       Task<FoodResponseDto> CreateFoodAsync(FoodCreateDto foodCreateDto, Stream imageStream, string originalFileName);
       Task DeleteFoodAsync(int foodId);
       Task UpdateFoodAsync(int foodId, FoodUpdateDto foodUpdateDto, Stream? imageStream = null, string? originalFileName = null);
       Task<PagedResponse<FoodResponseDto>> GetFoodsAsync(PaginationParams paginationParams,int? categoryId = null);
       Task<IReadOnlyList<FoodResponseDto>> GetFoodDetailsByIdsAsync(List<int> foodIds);
       Task<FoodResponseDto> GetFoodByIdAsync(int foodId);
       Task<PagedResponse<FoodResponseDto>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams);

    }
}