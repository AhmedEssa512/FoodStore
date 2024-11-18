using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Repository;

namespace FoodStore.Service.Abstracts
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId,CartItemDto cartItemDto);
        Task UpdateCartItemAsync(string userId,int cartItemId, int newQuantity);
        Task DeleteCartItemAsync(string userId,int cartItemId);
        Task DeleteCartItemsAsync(string userId,int cartId);
        Task<Cart> GetCartItemsAsync(string userId);
        
    }
}