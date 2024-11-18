using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService  categoryService)
        {
             _categoryService = categoryService;
        }


        [HttpPost("categories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAsync([FromBody]CategoryDto categoryDto)
        {
              if (!ModelState.IsValid)
               {
                    return BadRequest(ModelState); 
               }

             await  _categoryService.AddCategoryAsync(categoryDto);

             return Ok(new {Message ="Added successfully."});
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
           
             await _categoryService.DeleteCategoryAsync(id);
             return NoContent();
        }


        [HttpPut("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody] CategoryDto categoryDto)
        {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState); 
             }

             await _categoryService.UpdateCategoryAsync(id,categoryDto);

             return Ok(new {Message ="Updated successfully."});

        }

        [HttpGet("categories/{categoryId}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int categoryId)
        {
             var category = await _categoryService.GetCategoryAsync(categoryId);
             return Ok(category);
        }

            
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
	     var category = await _categoryService.GetCategoriesAsync();
             return Ok(category);
        }

    }
}