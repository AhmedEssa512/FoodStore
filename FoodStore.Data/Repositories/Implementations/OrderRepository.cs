using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
{
    public class OrderRepository : GenericBase<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string userId)
        {
            return await _context.orders
            .AsNoTracking()
            .Include(od => od.OrderDetails)
            .ThenInclude(f => f.Food)
            .Where(o => o.UserId == userId)
            .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.orders
            .AsNoTracking()
            .Include(o => o.OrderDetails)
            .ThenInclude(o => o.Food)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}