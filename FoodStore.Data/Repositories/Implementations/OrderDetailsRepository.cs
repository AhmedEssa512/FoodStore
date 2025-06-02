using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
{
    public class OrderDetailsRepository : GenericBase<OrderDetail>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OrderDetail?> GetOrderDetailsWithOrder(int orderItemId)
        {
            return await _context.orderDetails
            .Include(o => o.Order)
            .FirstOrDefaultAsync(od => od.Id == orderItemId);
        }
    }
}