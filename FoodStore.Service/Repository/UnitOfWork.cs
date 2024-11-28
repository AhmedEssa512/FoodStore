using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Service.Context;
using FoodStore.Service.IRepository;

namespace FoodStore.Service.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        public  IFoodRepository Food { get; }
        public  ICategoryRepository Category {get;}
        public IOrderRepository Order { get; }
        public IOrderDetailsRepository OrderDetails {get;}
        public ICartRepository Cart {get;}
        public ICartItemRepository CartItem {get;}

         public UnitOfWork(ApplicationDbContext context,
                      IFoodRepository foodRepository,
                      ICategoryRepository categoryRepository,
                      IOrderRepository orderRepository,
                      IOrderDetailsRepository orderDetailsRepository,
                      ICartRepository cartRepository,
                      ICartItemRepository cartItemRepository)
                {
                    _context = context ;
                    Food = foodRepository ;
                    Category = categoryRepository;
                    Order = orderRepository;
                    OrderDetails = orderDetailsRepository;
                    Cart = cartRepository;
                    CartItem = cartItemRepository;
                }



        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }

    }
}