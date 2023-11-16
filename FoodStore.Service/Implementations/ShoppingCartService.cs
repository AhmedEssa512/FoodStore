using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Implementations
{
    public class ShoppingCartService : GenericBase<Cart>, IShoppingCartService
    {
        private DbSet<Cart> _cart;
        public ShoppingCartService(ApplicationDbContext context) : base(context)
        {
            _cart = context.Set<Cart>();
        }

        public async Task<IQueryable<Cart>> GetCarts(string userId)
        {

            return  _cart.Where(c => c.UserId == userId).AsQueryable();
        }

        public async Task<Cart> IsFoodInCart(int foodId, string userId)
        {
             return  await _cart.SingleOrDefaultAsync(c => c.foodId == foodId  && c.UserId == userId);
        }

        public double GetShoppingCartTotal(string userId)
        {
            return _cart.Where(c => c.UserId == userId).Select(f => f.food.price * f.Amount).Sum();
        }

        public async Task<int> Count(string userId)
        {
          return  _cart.Where(c => c.UserId == userId).Count();
        }
    }
}