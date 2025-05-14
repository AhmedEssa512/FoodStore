using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Data.GenericRepository;
using FoodStore.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repository
{
    public class OrderRepository : GenericBase<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(string userId)
        {
            return await _context.orders
            .Include(od => od.OrderDetails)
            .Where(o => o.UserId == userId)
            .ToListAsync();
        }

        public async Task<Order?> GetOrderWithOrderDetailsAsync(int orderId)
        {
            return await _context.orders
            .Include(od => od.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        
    }
}