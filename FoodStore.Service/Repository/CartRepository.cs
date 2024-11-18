using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repository
{
    public class CartRepository : GenericBase<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartWithCartItemsAsync(string userId)
        {
            return await _context.carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        
        
      
    }
}