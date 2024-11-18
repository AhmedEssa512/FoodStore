using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repository
{
    public class CartItemRepository : GenericBase<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemWithCartAsync(int cartItemId)
        {
            return await _context.cartItems
            .Include(ci => ci.Cart) 
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }
    }
}