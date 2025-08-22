using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Context;
using FoodStore.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace FoodStore.Data.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork , IAsyncDisposable
    {
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public IFoodRepository Food { get; }
    public ICategoryRepository Category { get; }
    public IOrderRepository Order { get; }
    public IOrderDetailsRepository OrderDetails { get; }
    public ICartRepository Cart { get; }
    public ICartItemRepository CartItem { get; }
    public IUserRepository User { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IFoodRepository foodRepository,
        ICategoryRepository categoryRepository,
        IOrderRepository orderRepository,
        IOrderDetailsRepository orderDetailsRepository,
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IUserRepository userRepository
    )
    {
        _context = context;
        Food = foodRepository;
        Category = categoryRepository;
        Order = orderRepository;
        OrderDetails = orderDetailsRepository;
        Cart = cartRepository;
        CartItem = cartItemRepository;
        User = userRepository;
    }


    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No active transaction.");

        await _context.SaveChangesAsync();
        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
        }

        await _context.DisposeAsync();
    }

        public void Dispose()
        {
           DisposeAsync().GetAwaiter().GetResult();
        }
    }
}