using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
       


        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }


        [HttpPost("foods")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFoodAsync([FromBody]FoodDto foodDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            await _foodService.AddFoodAsync(foodDto);
        
            return Ok(new {Message ="Added successfully."});
        } 


        [HttpDelete("foods/{foodId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int foodId) 
        {
             await _foodService.DeleteFoodAsync(foodId);

             return NoContent();
        }


        [HttpPut("foods/{foodId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int foodId,[FromBody]FoodDto foodDto)
        {       
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            await _foodService.UpdateFoodAsync(foodId,foodDto);

            return Ok(new {Message ="Updated successfully."});
        }

        [HttpGet("foods/{foodId}")]
        public async Task<IActionResult> GetFoodByIdAsync(int foodId)
        {
            var food = await _foodService.GetFoodAsync(foodId);
           
            return Ok(food);
        }

        [HttpGet("foods")]
        public async Task<IActionResult> GetFoodsAsync()
        {
            return Ok(await _foodService.GetFoodsAsync());
        }








    }
}