using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Data.Entities.Enums;
using FoodStore.Contracts.DTOs.Order;
using FoodStore.Contracts.Interfaces;
using FoodStore.Contracts.Common;
using FoodStore.Api.Helpers;
using FoodStore.Contracts.QueryParams;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody]OrderDto orderDto)
        {
            if (!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            await _orderService.CreateOrderAsync(UserId,orderDto);
             return Ok(ApiResponse<string>.Ok("Order created successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int  id)
        {
            await _orderService.DeleteOrderAsync(UserId, id);
            return Ok(ApiResponse<string>.Ok("Order deleted successfully."));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            await _orderService.UpdateOrderAsync(UserId, id, orderDto);
             return Ok(ApiResponse<string>.Ok("Order updated successfully."));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var orders = await _orderService.GetUserOrdersAsync(UserId);
            return Ok(ApiResponse<IReadOnlyList<OrderListItemDto>>.Ok(orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(ApiResponse<OrderResponseDto>.Ok(order));
        }

        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId , [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId , request.NewStatus);
             return Ok(ApiResponse<OrderResponseDto>.Ok(order));
        }

        [HttpGet("orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrders([FromQuery]OrderQueryParameters query)
        {
            var result = await _orderService.GetAllOrdersForAdminAsync(
                query.PageNumber,
                query.PageSize,
                query.Status,
                query.StartDate,
                query.EndDate
            );

            return Ok(ApiResponse<PagedResponse<OrderListItemDto>>.Ok(result));
        }

   
    }
}