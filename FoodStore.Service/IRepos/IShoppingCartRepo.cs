using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepos
{
    public interface IShoppingCartRepo : IGenericBase<Cart>
    {
        public Task<Cart> IsFoodInCart(int foodId,string userId);
        public Task<IQueryable<Cart>> GetCarts (string userId);
        public double GetShoppingCartTotal (string userId);
        public Task<int> Count (string userId);



    }
}