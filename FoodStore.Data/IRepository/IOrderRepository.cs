using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.GenericRepository;

namespace FoodStore.Data.IRepository
{
    public interface IOrderRepository : IGenericBase<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersAsync(string userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);


    }
}