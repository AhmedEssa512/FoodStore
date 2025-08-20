using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}