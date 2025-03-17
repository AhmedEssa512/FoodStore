using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.GenericRepository;

namespace FoodStore.Data.IRepository
{
    public interface IOrderDetailsRepository : IGenericBase<OrderDetail>
    {
        Task<OrderDetail> GetOrderDetailsWithOrder(int orderItemId);
    }
}