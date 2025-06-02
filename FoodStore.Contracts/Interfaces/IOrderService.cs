using FoodStore.Contracts.DTOs.Order;

namespace FoodStore.Contracts.Interfaces
{
    public interface IOrderService 
    {
       Task AddOrderAsync(string userId,OrderDto orderDto);
       Task DeleteOrderAsync(string userId,int orderId);
       Task UpdateOrderAsync(string userId, int orderId, OrderDto orderDto);
       Task<OrderResponseDto> GetOrderByIdAsync(int orderId);
       Task<IReadOnlyList<OrderResponseDto>> GetUserOrdersAsync(string UserId);
       Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, string newStatus);


    }
}