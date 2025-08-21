using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Data.Projections;
using FoodStore.Data.Extensions;
using System.Linq.Expressions;

namespace FoodStore.Data.Repositories.Implementations
{
    public class OrderRepository : GenericBase<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<OrderSummary>> GetOrderSummariesAsync(string userId)
        {
            return await _context.orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .Select(o => new OrderSummary
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    Status = o.Status,
                    Total = o.Total,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<OrderWithDetails?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.orders
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderWithDetails
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    Address = o.Address,
                    Phone = o.Phone,
                    CreatedAt = o.CreatedAt,
                    Status = o.Status.ToString(),
                    Total = o.Total,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailProjection
                    {
                        FoodName = od.Food!.Name,
                        Price = od.UnitPrice,
                        Quantity = od.Quantity,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.orders.SumAsync(o => o.Total);
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.orders.CountAsync();
        }

        public async Task<PaginatedResult<OrderSummary>> GetAllOrders(
            int pageNumber, 
            int pageSize,
            Expression<Func<Order, bool>>? filter = null)
        {
            var query = _context.orders.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            var projection = query.Select(o => new OrderSummary
            {
                Id = o.Id,
                FullName = o.FullName, 
                Status = o.Status,
                Total = o.OrderDetails.Sum(i => i.Quantity * i.UnitPrice),
                CreatedAt = o.CreatedAt
            });

            var (Items, TotalCount) = await projection.ToPaginatedListAsync(pageNumber, pageSize);

            return new PaginatedResult<OrderSummary>(Items, TotalCount);
        }
    }
}