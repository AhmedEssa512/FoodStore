using FoodStore.Data.Entities;
using FoodStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Data.Repositories.Implementations
{
    public class CartItemRepository : GenericBase<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetCartItemWithCartAsync(int cartItemId)
        {
            return await _context.cartItems
            .Include(ci => ci.Cart) 
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }
    }
}