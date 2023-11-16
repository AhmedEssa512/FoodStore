using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Context;
using FoodStore.Service.GenericRepository;
using FoodStore.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Implementations
{
    public class OrderDetailsService : GenericBase<OrderDetail>, IOrderDetailsService
    {

        private readonly DbSet<OrderDetail> _orderDetails;
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartService _shoppingCartRepo;
        public OrderDetailsService(ApplicationDbContext context,IShoppingCartService shoppingCartRepo) : base(context)
        {
            _context = context;
            _shoppingCartRepo = shoppingCartRepo;
            _orderDetails = context.Set<OrderDetail>();
        }


        public async Task AddOrderDetails(Order order)
        {
                   
            var orderDetails = new List<OrderDetail>(await _shoppingCartRepo.Count(order.UserId));

            var cart = await _shoppingCartRepo.GetCarts(order.UserId);

            foreach (var item in cart)
            {
                orderDetails.Add(
                        new OrderDetail{
                            orderId = order.Id,
                            Food = item.food,
                            FoodId = item.foodId,
                            Amount = item.Amount
                           });
            }

    

        await _context.orderDetails.AddRangeAsync(orderDetails);
        await _context.SaveChangesAsync();

        
          
          
          
        
        }
    }
}