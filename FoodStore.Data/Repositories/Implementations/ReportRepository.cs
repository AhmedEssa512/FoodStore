using FoodStore.Data.Context;
using FoodStore.Data.Projections;
using FoodStore.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;
        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<SalesByCategoryProjection>> GetSalesByCategoryAsync()
        {
             return await _context.orderDetails
            .GroupBy(d => d.Food!.Category!.Name)
            .Select(g => new SalesByCategoryProjection
            {
                Category = g.Key,
                TotalQuantity = g.Sum(d => d.Quantity),
                Revenue = g.Sum(d => d.Quantity * d.UnitPrice)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync();
        }


        public async Task<IReadOnlyList<TopFoodProjection>> GetTopFoodsAsync(int topN)
        {
            return await _context.orderDetails
            .GroupBy(d => d.Food!.Name)
            .Select(g => new TopFoodProjection
            {
                FoodName = g.Key,
                QuantitySold = g.Sum(d => d.Quantity),
                Revenue = g.Sum(d => d.Quantity * d.UnitPrice)
            })
            .OrderByDescending(x => x.Revenue)
            .Take(topN)
            .ToListAsync();
        }


        public async Task<SalesSummaryProjection> GetSalesSummaryAsync()
        {
           return await _context.orders
            .SelectMany(o => o.OrderDetails, (o, d) => new
            {
                o.Id,
                o.Total,
                d.Quantity
            })
            .GroupBy(x => 1)
            .Select(g => new SalesSummaryProjection
            {
                TotalSales = g.Sum(x => x.Total),
                TotalOrders = g.Select(x => x.Id).Distinct().Count(),
                TotalItems = g.Sum(x => x.Quantity)
            })
            .FirstOrDefaultAsync()
            ?? new SalesSummaryProjection(); // fallback for empty DB
            }
    }
}