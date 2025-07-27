using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Cart;
using FoodStore.Contracts.Interfaces;
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
                await _cartService.AddToCartAsync(UserId, cartItemDto);
                return Ok(ApiResponse<string>.Ok("Item added to cart successfully."));
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetCartAsync()
        {
                var cart = await _cartService.GetUserCartAsync(UserId);
                return Ok(ApiResponse<CartResponseDto>.Ok(cart));
        }


        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartAsync(int cartItemId, [FromBody]CartItemUpdateDto dto)
        {
               await _cartService.UpdateCartItemAsync(UserId,cartItemId,dto.Quantity);
               return Ok(ApiResponse<string>.Ok("Cart updated successfully"));
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItemAsync(int cartItemId)
        {
                await _cartService.DeleteCartItemAsync(UserId,cartItemId);
                return Ok(ApiResponse<string>.Ok("Cart item deleted successfully"));
        }

        [HttpDelete("items")]
        public  async Task<IActionResult> DeleteAllItemAsync()
        {
                await _cartService.DeleteCartItemsAsync(UserId);
                return Ok(ApiResponse<string>.Ok("Cart deleted successfully"));
        }

        [HttpPost("merge")]
        [Authorize]
        public async Task<IActionResult> MergeCart([FromBody] List<CartItemDto> items)
        {
               await _cartService.MergeCartAsync(UserId, items);
               return Ok(ApiResponse<string>.Ok("Cart merged successfully."));
        }


    }
}