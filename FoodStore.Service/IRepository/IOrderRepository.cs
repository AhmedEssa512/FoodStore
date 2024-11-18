using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepository
{
    public interface IOrderRepository : IGenericBase<Order>
    {
        Task<Order> GetOrderWithOrderDetailsAsync(int OrderId);
        Task<List<Order>> GetOrdersAsync(string userId);

    }
}