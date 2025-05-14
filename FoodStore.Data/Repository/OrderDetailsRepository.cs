using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Data.GenericRepository;
using FoodStore.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Repository
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