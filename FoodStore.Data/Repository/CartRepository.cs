using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using FoodStore.Data.GenericRepository;
using FoodStore.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Repository
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
            .ThenInclude(f => f.Food)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        
        
      
    }
}