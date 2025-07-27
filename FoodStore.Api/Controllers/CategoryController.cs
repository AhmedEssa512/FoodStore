using FoodStore.Api.Helpers;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Category;
using FoodStore.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CategoryDto categoryDto)
        {
             if (!ModelState.IsValid)
               return BadRequest(ApiResponseHelper.FromModelState(ModelState)); 
               
             await  _categoryService.AddCategoryAsync(categoryDto);
             return Ok(ApiResponse<string>.Ok("Category created successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
             await _categoryService.DeleteCategoryAsync(id);
             return Ok(ApiResponse<string>.Ok("Category deleted successfully"));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,[FromBody] CategoryDto categoryDto)
        {
             if (!ModelState.IsValid)
                 return BadRequest(ApiResponseHelper.FromModelState(ModelState)); 

             await _categoryService.UpdateCategoryAsync(id,categoryDto);
             return Ok(ApiResponse<string>.Ok("Category updated successfully"));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
             var category = await _categoryService.GetCategoryByIdAsync(id);
             return Ok(ApiResponse<CategoryResponseDto>.Ok(category));
        }

            
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
	     var categories = await _categoryService.GetCategoriesAsync();
             return Ok(ApiResponse<IReadOnlyList<CategoryResponseDto>>.Ok(categories));
        }

    }
}