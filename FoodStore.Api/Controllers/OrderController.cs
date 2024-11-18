using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
// using FoodStore.Service.DTOS;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("orders")]
        public async Task<IActionResult> AddOrderAsync([FromBody]OrderDto orderDto){

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.AddOrderAsync(userId,orderDto);

            return Ok(new {Message ="Added successfully."});
        }

        [HttpDelete("orders/{orderId}")]
        public async Task<IActionResult> DeleteOrderAsync(int  orderId){

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.DeleteOrderAsync(userId,orderId);

            return NoContent();
        }

        [HttpDelete("orders/items/{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItemAsync(int  orderItemId){

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.DeleteOrderItemAsync(userId,orderItemId);

            return NoContent();
        }

        [HttpPut("orders/items/{orderItemId}")]
        public async Task<IActionResult> UpdateOrderAsync(int orderItemId,[FromBody] int quantity)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _orderService.UpdateOrderAsync(userId,orderItemId,quantity);

            return Ok(new {Message ="Updated successfully."});
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersAsync()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _orderService.GetOrdersAsync(userId);

            return Ok(orders);
        }


   
    }
}