using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.Abstracts
{
    public interface IOrderService 
    {
       Task AddOrderAsync(string userId,OrderDto orderDto);
       Task DeleteOrderAsync(string userId,int orderId);
       Task DeleteOrderItemAsync(string userId,int orderItemId);
       Task UpdateOrderAsync(string userId,int orderItemId,int quantity);
       Task<List<Order>> GetOrdersAsync(string UserId);


    }
}