using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.DTOs.Reports;
using FoodStore.Contracts.Interfaces;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository  _reportRepo;
        public ReportService(IReportRepository  reportRepo)
        {
             _reportRepo = reportRepo;
        }
        public async Task<IReadOnlyList<SalesByCategoryDto>> GetSalesByCategoryAsync()
        {
            var projections = await _reportRepo.GetSalesByCategoryAsync();
            var totalRevenue = projections.Sum(p => p.Revenue);

            return projections.Select(p => new SalesByCategoryDto
            {
                Category = p.Category,
                TotalQuantity = p.TotalQuantity,
                Revenue = p.Revenue,
                RevenueFormatted = p.Revenue.ToString("C"), // C stands for Currency format string.
                PercentageOfTotal = totalRevenue == 0 ? 0 : (double)(p.Revenue / totalRevenue * 100)
            }).ToList();
        }

        public async Task<IReadOnlyList<TopFoodDto>> GetTopFoodsAsync(int topN)
        {
            var projections = await _reportRepo.GetTopFoodsAsync(topN);

            return projections.Select((p, index) => new TopFoodDto
            {
                Rank = index + 1,
                FoodName = p.FoodName,
                QuantitySold = p.QuantitySold,
                Revenue = p.Revenue,
                RevenueFormatted = p.Revenue.ToString("C")
            }).ToList();
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync()
        {
            var projection = await _reportRepo.GetSalesSummaryAsync();

            return new SalesSummaryDto
            {
                TotalSales = projection.TotalSales,
                TotalOrders = projection.TotalOrders,
                TotalItems = projection.TotalItems,
                TotalSalesFormatted = projection.TotalSales.ToString("C"),
                GeneratedAt = DateTime.UtcNow
            };
        }
    }
}