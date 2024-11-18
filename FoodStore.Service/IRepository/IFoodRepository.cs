using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepository
{
    public interface IFoodRepository : IGenericBase<Food>
    {
        Task<List<Food>> GetFoodsAsync();
        Task<bool> AnyFoodAsync(int foodId);
        Task<double> GetPriceAsync(int foodId);
    }
}