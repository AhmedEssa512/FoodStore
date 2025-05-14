using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Data.GenericRepository;

namespace FoodStore.Data.IRepository
{
    public interface IFoodRepository : IGenericBase<Food>
    {
        Task<bool> AnyFoodAsync(int foodId);
        Task<decimal> GetPriceAsync(int foodId);
        Task<(IReadOnlyList<Food>, int TotalCount)> GetPaginatedFoods(PaginationParams paginationParams,int? categoryId = null);
        Task<(IReadOnlyList<Food>, int TotalCount)> SearchFoodsInDatabaseAsync(string searchQuery, PaginationParams paginationParams);
        Task<IEnumerable<Food>> GetFoodsByIdsAsync(List<int> foodIds);
    }
}