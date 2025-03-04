using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepository
{
    public interface IFoodRepository : IGenericBase<Food>
    {
        Task<bool> AnyFoodAsync(int foodId);
        Task<double> GetPriceAsync(int foodId);
        Task<IEnumerable<Food>> GetPaginatedFoods(PaginationParams paginationParams);
    }
}