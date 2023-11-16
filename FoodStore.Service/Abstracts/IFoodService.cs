using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.Abstracts
{
    public interface IFoodService : IGenericBase<Food>
    {
       public Task<List<Food>> GetFoodsAsync();
       public Task<bool> IsFoundFoodId(int foodId);
    }
}