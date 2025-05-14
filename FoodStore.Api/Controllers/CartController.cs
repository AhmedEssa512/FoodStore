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
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        {
                await _cartService.AddToCartAsync(UserId,cartItemDto);
                return Ok(new {Message ="Added successfully."});
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetCartAsync()
        {
                var cart = await _cartService.GetCartItemsAsync(UserId);
                return Ok(cart);
        }


        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartAsync(int cartItemId, [FromBody]CartItemUpdateDto dto)
        {
               await _cartService.UpdateCartItemAsync(UserId,cartItemId,dto.Quantity);
               return Ok(new { Message = "Updated successfully." });
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItemAsync(int cartItemId)
        {
                await _cartService.DeleteCartItemAsync(UserId,cartItemId);
                return NoContent();
        }

        [HttpDelete("items")]
        public  async Task<IActionResult> DeleteAllItemAsync()
        {
                await _cartService.DeleteCartItemsAsync(UserId);
                return NoContent();
        }

        [HttpPost("merge")]
        [Authorize]
        public async Task<IActionResult> MergeCart([FromBody] List<CartItemDto> items)
        {
               await _cartService.MergeCartAsync(UserId, items);
               return Ok();
        }


    }
}