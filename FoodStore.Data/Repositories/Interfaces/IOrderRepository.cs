using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.Projections;

namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericBase<Order>
    {
        Task<IReadOnlyList<OrderSummary>> GetOrderSummariesAsync(string userId);
        Task<OrderWithDetails?> GetOrderWithDetailsAsync(int orderId);
        Task<decimal> GetTotalSalesAsync();
        Task<int> GetTotalOrdersAsync();
        Task<PaginatedResult<OrderSummary>> GetAllOrders(
            int pageNumber, 
            int pageSize,
            Expression<Func<Order, bool>>? filter = null);
    }
}