using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderDto orderDto)
        {
            await _orderService.AddOrderAsync(UserId,orderDto);

            return Ok(new { Message = "Added successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int  id)
        {
            await _orderService.DeleteOrderAsync(UserId, id);

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderDto)
        {
            await _orderService.UpdateOrderAsync(UserId, id, orderDto);

            return Ok(new { Message = "Updated successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetOrdersAsync(UserId);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            return Ok(order);
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId , [FromBody] OrderStatus newStatus)
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId , newStatus);

            return Ok(order);
        }

   
    }
}