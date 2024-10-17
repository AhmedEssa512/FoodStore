using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCart;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IFoodService _food;


        public ShoppingCartController(IShoppingCartService shoppingCart,IFoodService food,UserManager<ApplicationUser> usermanager)
        {
            _shoppingCart = shoppingCart;
            _food = food;
            _usermanager = usermanager;
        }



        [HttpPost("Add/{foodId}")]
        public async Task<IActionResult> AddAsync(int foodId,[FromForm]int amount)
        {

            if(amount <= 0 ) return BadRequest("Invalid amount");

          if(! await _food.IsFoundFoodId(foodId))
            return NotFound("Not found food");

        
            var userId = HttpContext.User.FindFirstValue("uid");



            var ExistIncart = await _shoppingCart.IsFoodInCart(foodId,userId);
            if(ExistIncart is null)
            {
                var cart = new Cart{
                    foodId = foodId,
                    UserId = userId, 
                    Amount = amount
                };

                await _shoppingCart.AddAsync(cart);
            }
            else
            {
                ExistIncart.Amount += amount;
               await _shoppingCart.UpdateAsync(ExistIncart);
            }




            return Ok("succeeded");
        }

        [HttpDelete("Delete/{cartId}")]
        public async Task<IActionResult> DeleteAync(int cartId)
        {
            var cart = await _shoppingCart.GetByIdAsync(cartId);

            if(cart is null) return NotFound("Not found Cart");

           await _shoppingCart.DeleteAsync(cart);

           return Ok("succeeded");
        }

        [HttpDelete("DeleteRangeAsync")]
        public async Task<IActionResult> DeleteRangeAync()
        {
            var carts = await _shoppingCart.GetCarts(HttpContext.User.FindFirstValue("uid"));
            if(carts.Count() == 0) return BadRequest("No cart items found!");

            await _shoppingCart.DeleteRangeAsync(carts.ToList());

            return Ok(carts);
        }

        [HttpPut("Update/{cartId}")]
        public async Task<IActionResult> UpdateAsync(int cartId,[FromForm] int amount)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);
            var cart = await _shoppingCart.GetByIdAsync(cartId);

            if(cart is null) return NotFound("Not found Cart");

            if(amount <= 0 ) return BadRequest("Not valid amount");

            cart.Amount = amount ; 

           await _shoppingCart.UpdateAsync(cart);

           return Ok("succeeded");
        }

        [HttpGet("GetCarts")]
         public async Task<IActionResult> GetCarts()
         {
            return Ok(await _shoppingCart.GetCarts(HttpContext.User.FindFirstValue("uid")));
         }

         [HttpGet("GetTotal")]
         public IActionResult GetTotal()
         {
            return Ok( _shoppingCart.GetShoppingCartTotal(HttpContext.User.FindFirstValue("uid")) );
         }


         [HttpGet("count")]
         public IActionResult Count()
         {
            return Ok(_shoppingCart.Count(HttpContext.User.FindFirstValue("uid")));
         }








    }
}