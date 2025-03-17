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
        Task<Order> GetOrderWithOrderDetailsAsync(int OrderId);
        Task<List<Order>> GetOrdersAsync(string userId);

    }
}