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
       Task<Food> CreateFoodAsync(Food food);
       Task DeleteFoodAsync(int foodId);
       Task UpdateFoodAsync(int foodId,FoodDto foodDto);
       Task<IEnumerable<Food>> GetFoodsAsync(PaginationParams paginationParams,int? categoryId = null);
       Task<IEnumerable<Food>> GetFoodDetailsByIdsAsync(List<int> foodIds);
       Task<Food> GetFoodAsync(int foodId);
       Task<IReadOnlyList<Food>> SearchFoodsAsync(string searchQuery, PaginationParams paginationParams);

    }
}