using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.IRepos;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Repos
{
    public class OrderRepo : GenericBase<Order>, IOrderRepo
    {
            private readonly DbSet<Order> _Order;
 


        public OrderRepo(ApplicationDbContext context,IShoppingCartRepo shoppingCartRepo,IOrderDetailsRepo orderDetails) : base(context)
        {
            _Order = context.Set<Order>();

        
        }

        
    }
}