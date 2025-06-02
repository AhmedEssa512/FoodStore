using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
{
    public class CartRepository : GenericBase<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartWithCartItemsAsync(string userId)
        {
            var cart = await _context.carts
            .Include(c => c.Items)
            .ThenInclude(f => f.Food)
            .FirstOrDefaultAsync(c => c.UserId == userId);
            
            return cart;
        }

        
        
      
    }
}