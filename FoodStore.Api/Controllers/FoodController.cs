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
        public async Task<IActionResult> Create([FromForm]FoodDto foodDto)
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
        public async Task<IActionResult> Delete(int foodId) 
        {
             await _foodService.DeleteFoodAsync(foodId);

             return NoContent();
        }


        [HttpPut("foods/{foodId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int foodId,[FromForm]FoodDto foodDto)
        {       
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            await _foodService.UpdateFoodAsync(foodId,foodDto);

            return Ok(new {Message ="Updated successfully."});
        }


        [HttpGet("foods")]
        public async Task<IActionResult> GetFoods([FromQuery] FoodQueryParams queryParams)
        {
            var pagination = new PaginationParams
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var foods = await _foodService.GetFoodsAsync(pagination, queryParams.CategoryId);
            return Ok(foods);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string query,[FromQuery]PaginationParams paginationParams)
        {
            var data = await _foodService.SearchFoodsAsync(query,paginationParams);
            
            return Ok(data);
        }

        [HttpPost("getFoodByIds")]
        public async Task<IActionResult> getFoodByIds([FromBody]List<int> Ids)
        {
            var foods = await _foodService.GetFoodDetailsByIdsAsync(Ids);
            return Ok(foods);
        }        








    }
}