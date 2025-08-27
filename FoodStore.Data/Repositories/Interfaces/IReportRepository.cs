using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Projections;

namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IReadOnlyList<SalesByCategoryProjection>> GetSalesByCategoryAsync();
        Task<IReadOnlyList<TopFoodProjection>> GetTopFoodsAsync(int topN);
        Task<SalesSummaryProjection> GetSalesSummaryAsync();
    }
}