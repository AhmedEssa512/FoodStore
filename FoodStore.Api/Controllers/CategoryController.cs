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
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<CategoryController> _localizer;

        public CategoryController(ICategoryService category,IStringLocalizer<CategoryController> localizer)
        {
            _category = category;
            _localizer = localizer;
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromForm]CategoryDto categoryDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            var category = new Category();
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

             await  _category.AddAsync(category);

            return Ok(category);
        }

        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _category.GetByIdAsync(id);

            if(category is null)
             return NotFound(_localizer["CategoryIsNotFound"].Value);

             await _category.DeleteAsync(category);

             return Ok(_localizer["Deleted"].Value);
        }


        [HttpPut("UpdateAsync/{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm] CategoryDto categoryDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            var category = await _category.GetByIdAsync(id);

            if(category is null)
             return NotFound(_localizer["CategoryIsNotFound"].Value);

             category.Name = categoryDto.Name;
             category.Description = categoryDto.Description;

             await _category.UpdateAsync(category);

             return Ok(_localizer["Updated"].Value);

        }

        [HttpGet("GetCategoryByIdAsync")]
        public async Task<IActionResult> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _category.GetByIdAsync(categoryId);
            if(category is null) return NotFound(_localizer["CategoryIsNotFound"].Value);

            return Ok(category);
        }

            
            [HttpGet("GetCategoriesAsync")]
            // [Authorize]
            public async Task<IActionResult> GetCategoriesAsync()
            {
                return Ok(await _category.GetCategoriesAsync());
            }

    }
}