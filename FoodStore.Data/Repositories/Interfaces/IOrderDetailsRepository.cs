using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;


namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IOrderDetailsRepository : IGenericBase<OrderDetail>
    {
        Task<OrderDetail?> GetOrderDetailsWithOrder(int orderItemId);
    }
}