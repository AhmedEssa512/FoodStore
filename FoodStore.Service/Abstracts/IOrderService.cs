using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;

namespace FoodStore.Service.Abstracts
{
    public interface IOrderService 
    {
       Task AddOrderAsync(string userId,OrderDto orderDto);
       Task DeleteOrderAsync(string userId,int orderId);
       Task UpdateOrderAsync(string userId, int orderId, OrderDto orderDto);
       Task<OrderResponseDto> GetOrderByIdAsync(int orderId);
       Task<IReadOnlyList<Order>> GetOrdersAsync(string UserId);
       Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);


    }
}