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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFoodAsync([FromForm]FoodDto foodDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

           var food = await _foodService.CreateFoodAsync(foodDto);
        
            return CreatedAtAction(
                   nameof(GetById),  
                   new { id = food.Id },  
                   food  
                 );
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var food = await _foodService.GetFoodAsync(id);

            if (food == null)
            {
                return NotFound();
            }
           
            return Ok(food);
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
        public async Task<IActionResult> UpdateAsync(int foodId,[FromForm]FoodDto foodDto)
        {       
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            await _foodService.UpdateFoodAsync(foodId,foodDto);

            return Ok(new {Message ="Updated successfully."});
        }


        [HttpGet("foods")]
        public async Task<IActionResult> GetFoodsAsync([FromQuery]PaginationParams paginationParams)
        {
            return Ok(await _foodService.GetFoodsAsync(paginationParams));
        }








    }
}