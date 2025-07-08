using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;
using FoodStore.Data.Projections;

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
    }
}