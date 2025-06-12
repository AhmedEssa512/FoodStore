using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Data.Entities.Enums;
using FoodStore.Contracts.DTOs.Order;
using FoodStore.Contracts.Interfaces;

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
            await _orderService.CreateOrderAsync(UserId,orderDto);

            return Ok(new { Message = "Added successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int  id)
        {
            await _orderService.DeleteOrderAsync(UserId, id);
            return NoContent();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderDto)
        {
            await _orderService.UpdateOrderAsync(UserId, id, orderDto);

            return Ok(new { Message = "Updated successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var orders = await _orderService.GetUserOrdersAsync(UserId);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            return Ok(order);
        }

        [HttpPatch("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId , [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId , request.NewStatus);

            return Ok(order);
        }

   
    }
}