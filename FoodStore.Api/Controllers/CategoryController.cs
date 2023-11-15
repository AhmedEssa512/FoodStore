using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _category;

        public CategoryController(ICategoryRepo category)
        {
            _category = category;
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
             return NotFound("Category Not found");

             await _category.DeleteAsync(category);

             return Ok("succeeded");
        }


        [HttpPut("UpdateAsync/{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm] CategoryDto categoryDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            var category = await _category.GetByIdAsync(id);

            if(category is null)
             return NotFound("Category Not found");

             category.Name = categoryDto.Name;
             category.Description = categoryDto.Description;

             await _category.UpdateAsync(category);

             return Ok("succeeded");

        }

            
            [HttpGet("GetCategoriesAsync")]
            [Authorize]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> GetCategoriesAsync()
            {
                return Ok(await _category.GetCategoriesAsync());
            }

    }
}