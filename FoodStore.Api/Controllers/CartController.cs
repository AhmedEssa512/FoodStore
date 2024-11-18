using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }
                  string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
                  await _cartService.AddToCartAsync(userId,cartItemDto);

                  return Ok(new {Message ="Added successfully."});
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetCartAsync(){

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var cart = await _cartService.GetCartItemsAsync(userId) ;
                 return Ok(cart);
        }


        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartAsync(int cartItemId,[FromBody]int quantity){

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }

               string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
               await _cartService.UpdateCartItemAsync(userId,cartItemId,quantity);

               return Ok(new {Message ="Updated successfully."});
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItemAsync(int cartItemId)
        {
            
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
                await _cartService.DeleteCartItemAsync(userId,cartItemId);
 
                return NoContent();;     
        }

         [HttpDelete("items")]
        public  async Task<IActionResult> DeleteAllItemAsync(int cartId)
        {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _cartService.DeleteCartItemsAsync(userId,cartId);
                
                return NoContent();
        }

    }
}