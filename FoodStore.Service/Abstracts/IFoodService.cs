using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.Abstracts
{
    public interface IFoodService 
    {
       Task<Food> AddFoodAsync(FoodDto foodDto);
       Task DeleteFoodAsync(int foodId);
       Task UpdateFoodAsync(int foodId,FoodDto foodDto);
       Task<List<Food>> GetFoodsAsync();
       Task<Food> GetFoodAsync(int foodId);

    }
}