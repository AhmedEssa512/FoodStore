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
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService  categoryService)
        {
             _categoryService = categoryService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CategoryDto categoryDto)
        {

             await  _categoryService.AddCategoryAsync(categoryDto);

             return Ok(new { Message ="Added successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
           
             await _categoryService.DeleteCategoryAsync(id);
             return NoContent();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,[FromBody] CategoryDto categoryDto)
        {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState); 
             }

             await _categoryService.UpdateCategoryAsync(id,categoryDto);

             return Ok(new { Message ="Updated successfully." });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
             var category = await _categoryService.GetCategoryAsync(id);
             return Ok(category);
        }

            
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
	     var category = await _categoryService.GetCategoriesAsync();
             return Ok(category);
        }

    }
}