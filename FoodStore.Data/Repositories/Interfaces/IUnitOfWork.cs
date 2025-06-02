using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IUnitOfWork 
    {
       public IFoodRepository Food { get; }
       public ICategoryRepository Category {get;}
       public IOrderRepository Order { get; }
       public IOrderDetailsRepository OrderDetails {get;}
       public ICartRepository Cart {get;}
       public ICartItemRepository CartItem {get;}

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}