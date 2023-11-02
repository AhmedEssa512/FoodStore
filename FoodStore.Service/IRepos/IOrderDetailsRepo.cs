using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.GenericRepository;

namespace FoodStore.Service.IRepos
{
    public interface IOrderDetailsRepo : IGenericBase<OrderDetail>
    {
        public Task AddOrderDetails(Order order); 
    }
}