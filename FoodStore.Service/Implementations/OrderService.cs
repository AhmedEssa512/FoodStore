using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Implementations
{
    public class OrderService : GenericBase<Order>, IOrderService
    {
            private readonly DbSet<Order> _Order;
 


        public OrderService(ApplicationDbContext context,IShoppingCartService shoppingCartRepo,IOrderDetailsService orderDetails) : base(context)
        {
            _Order = context.Set<Order>();

        
        }

        public Task<IEnumerable<Order>> GetOrders(string userId)
        {
            throw new NotImplementedException();
        }
    }
}