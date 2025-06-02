using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;

namespace FoodStore.Data.Repositories.Interfaces
{
    public interface ICartItemRepository:IGenericBase<CartItem>
    {
        Task<CartItem?> GetCartItemWithCartAsync(int cartItemId);
    }
}