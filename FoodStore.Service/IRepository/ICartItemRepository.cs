using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepository
{
    public interface ICartItemRepository:IGenericBase<CartItem>
    {
        Task<CartItem> GetCartItemWithCartAsync(int cartItemId);
    }
}