using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Order;

namespace FoodStore.Contracts.Interfaces
{
    public interface IOrderService 
    {
       Task CreateOrderAsync(string userId,OrderDto orderDto);
       Task DeleteOrderAsync(string userId,int orderId);
       Task UpdateOrderAsync(string userId, int orderId, OrderDto orderDto);
       Task<OrderResponseDto> GetOrderByIdAsync(int orderId);
       Task<IReadOnlyList<OrderListItemDto>> GetUserOrdersAsync(string UserId);
       Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus);
       Task<PagedResponse<OrderListItemDto>> GetAllOrdersForAdminAsync(
            int pageNumber,
            int pageSize,
            string? statusFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null);


    }
}