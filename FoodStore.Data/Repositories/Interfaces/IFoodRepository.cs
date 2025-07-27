using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;


namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IFoodRepository : IGenericBase<Food>
    {
        Task<bool> AnyFoodAsync(int foodId);
        Task<decimal> GetPriceAsync(int foodId);
        Task<(IReadOnlyList<Food>, int TotalCount)> GetPaginatedFoods(int pageNumber, int pageSize,int? categoryId = null);
        Task<(IReadOnlyList<Food>, int TotalCount)> SearchFoodsInDatabaseAsync(string searchQuery, int pageNumber, int pageSize);
        Task<IReadOnlyList<Food>> GetFoodsByIdsAsync(List<int> foodIds);
    }
}