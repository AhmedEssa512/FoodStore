using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.DTOs.Reports;

namespace FoodStore.Contracts.Interfaces
{
    public interface IReportService
    {
        Task<IReadOnlyList<SalesByCategoryDto>> GetSalesByCategoryAsync();
        Task<IReadOnlyList<TopFoodDto>> GetTopFoodsAsync(int topN);
        Task<SalesSummaryDto> GetSalesSummaryAsync();
    }
}