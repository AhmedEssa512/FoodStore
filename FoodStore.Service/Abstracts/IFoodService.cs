using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace FoodStore.Service.Abstracts
{
    public interface IFoodService 
    {
       Task<Food> CreateFoodAsync(FoodCreateDto foodCreateDto);
       Task DeleteFoodAsync(int foodId);
       Task UpdateFoodAsync(int foodId, FoodUpdateDto foodDto);
       Task<PaginatedResult<Food>> GetFoodsAsync(PaginationParams paginationParams,int? categoryId = null);
       Task<IEnumerable<Food>> GetFoodDetailsByIdsAsync(List<int> foodIds);
       Task<Food> GetFoodAsync(int foodId);
       Task<PaginatedResult<Food>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams);

    }
}