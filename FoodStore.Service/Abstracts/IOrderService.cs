using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.Abstracts
{
    public interface IOrderService : IGenericBase<Order>
    {
        Task<IEnumerable<Order>> GetOrders(string userId);
    }
}