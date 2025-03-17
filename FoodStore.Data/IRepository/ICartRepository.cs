using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Data.GenericRepository;

namespace FoodStore.Data.IRepository
{
    public interface ICartRepository:IGenericBase<Cart>
    {
       Task<Cart> GetCartWithCartItemsAsync(string userId);
       
    }
}