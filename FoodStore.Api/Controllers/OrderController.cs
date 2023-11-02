using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.DTOS;
using FoodStore.Service.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _order;
        private readonly IOrderDetailsRepo _orderDetails;

        private readonly IShoppingCartRepo _shoppingCart;


        public OrderController(IOrderRepo order,IShoppingCartRepo shoppingCart,IOrderDetailsRepo orderDetails)
        {
            _order = order ;
            _orderDetails = orderDetails;
            _shoppingCart = shoppingCart;
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromForm] OrderDto orderDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var order = new Order{
                Address = orderDto.Address,
                Phone = orderDto.Phone,
                UserId = HttpContext.User.FindFirstValue("uid"),
                Total = _shoppingCart.GetShoppingCartTotal(HttpContext.User.FindFirstValue("uid"))
            };

            await _order.AddAsync(order); 
            await _orderDetails.AddOrderDetails(order);

            return Ok("Succeeded");
            
        }


        [HttpDelete("Delete/{orderId}")]
        public async Task<IActionResult> DeleteAsync(int orderId)
        {
            var order = await _order.GetByIdAsync(orderId);

            if(order is null)
             return BadRequest("Not found order");

             await _order.DeleteAsync(order);

             return Ok("Succeeded");
        }
    }
}