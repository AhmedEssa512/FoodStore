using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.IRepos;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepo _food;
        private readonly ICategoryRepo _category;


        public FoodController(IFoodRepo food,ICategoryRepo category)
        {
            _food = food;
            _category = category;
        }


        [HttpPost("AddFoodAsync")]
        public async Task<IActionResult> AddFoodAsync([FromForm]FoodDto foodDto)
        {
            if( !await _category.IsFoundCategory(foodDto.CategoryId) )
             return BadRequest("Not found category with this name!");

                 
              var food = new Food();

             if(foodDto.Photo is not null)
             {
                 using var  DataStream = new MemoryStream();
                 await foodDto.Photo.CopyToAsync(DataStream);

                 food.Photo = DataStream.ToArray();
            }


            food.Name = foodDto.Name;
            food.Description = foodDto.Description;
            food.price = foodDto.price;
            food.CategoryId = foodDto.CategoryId;
            
            await _food.AddAsync(food);
            
            return Ok(food);
        } 


        [HttpDelete("DeleteAsync/{foodId}")]
        public async Task<IActionResult> DeleteAsync(int foodId) 
        {
            var food = await _food.GetByIdAsync(foodId);

            if(food is null)
             return BadRequest("Not found food");

             await _food.DeleteAsync(food);

            return Ok("Succeeded");
        }



        [HttpPut("UpdateAsync/{foodId}")]
        public async Task<IActionResult> UpdateAsync(int foodId,[FromForm]FoodDto foodDto)
        {

                var food = await _food.GetByIdAsync(foodId);

                if(food is null)
                 return BadRequest("Not found food");

                if( !await _category.IsFoundCategory(foodDto.CategoryId) )
                  return BadRequest("Not found category with this name!");

                 

             if(foodDto.Photo is not null)
             {
                 using var  DataStream = new MemoryStream();
                 await foodDto.Photo.CopyToAsync(DataStream);

                 food.Photo = DataStream.ToArray();
            }


            food.Name = foodDto.Name;
            food.Description = foodDto.Description;
            food.price = foodDto.price;
            food.CategoryId = foodDto.CategoryId;
            
            await _food.UpdateAsync(food);


            return Ok("Succeeded");
        }

        [HttpGet("GetFoodsAsync")]
        public async Task<IActionResult> GetFoodsAsync()
        {
            return Ok(await _food.GetFoodsAsync());
        }








    }
}