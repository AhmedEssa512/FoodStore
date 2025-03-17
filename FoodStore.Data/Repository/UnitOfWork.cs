using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Context;
using FoodStore.Data.IRepository;
using FoodStore.Service.Repository;

namespace FoodStore.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
    private readonly ApplicationDbContext _context;

    private IFoodRepository _foodRepository;
    private ICategoryRepository _categoryRepository;
    private IOrderRepository _orderRepository;
    private IOrderDetailsRepository _orderDetailsRepository;
    private ICartRepository _cartRepository;
    private ICartItemRepository _cartItemRepository;

    // Expose repositories as properties
    public IFoodRepository Food => _foodRepository ??= new FoodRepository(_context);
    public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(_context);
    public IOrderRepository Order => _orderRepository ??= new OrderRepository(_context);
    public IOrderDetailsRepository OrderDetails => _orderDetailsRepository ??= new OrderDetailsRepository(_context);
    public ICartRepository Cart => _cartRepository ??= new CartRepository(_context);
    public ICartItemRepository CartItem => _cartItemRepository ??= new CartItemRepository(_context);

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
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