
namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IUnitOfWork :  IDisposable
    {
       public IFoodRepository Food { get; }
       public ICategoryRepository Category {get;}
       public IOrderRepository Order { get; }
       public IOrderDetailsRepository OrderDetails {get;}
       public ICartRepository Cart {get;}
       public ICartItemRepository CartItem {get;}
       public IUserRepository User {get;}

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}