using FoodStore.Api.Helpers;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Food;
using FoodStore.Contracts.Interfaces;
using FoodStore.Contracts.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IImageService _imageService;

        public FoodController(IFoodService foodService, IImageService imageService)
        {
            _foodService = foodService;
            _imageService = imageService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(FoodResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromForm] FoodCreateDto foodCreateDto , IFormFile file)
        {
            if (foodCreateDto == null)
                return BadRequest(ApiResponse<string>.Fail("Food data is required."));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("Image is required."));

            using var stream = file.OpenReadStream();

            var createdFood = await _foodService.CreateFoodAsync(foodCreateDto, stream, file.FileName);
        
            return CreatedAtAction(
                   nameof(GetById),  
                   new { id = createdFood.Id },  
                   ApiResponse<FoodResponseDto>.Ok(createdFood, "Food created successfully")  
                 );
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodDto = await _foodService.GetFoodByIdAsync(id);
            return Ok(ApiResponse<FoodResponseDto>.Ok(foodDto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
             await _foodService.DeleteFoodAsync(id);
             return Ok(ApiResponse<string>.Ok("Food deleted successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] FoodUpdateDto foodUpdateDto, IFormFile? file = null)
        {       
            if (foodUpdateDto == null)
                return BadRequest(ApiResponse<string>.Fail("Food data is required."));

            if (!ModelState.IsValid)
                return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            Stream? imageStream = null;
            string? originalFileName = null; 

            if(file != null)
            {
               imageStream = file.OpenReadStream();
               originalFileName = file.FileName;
            }

            await _foodService.UpdateFoodAsync(id, foodUpdateDto, imageStream, originalFileName);

            return Ok(ApiResponse<string>.Ok("Food updated successfully."));
        }

        [HttpPatch("{foodId}/availability")]
        public async Task<IActionResult> UpdateAvailability(int foodId, [FromBody] bool isAvailable)
        {
            await _foodService.UpdateFoodAvailabilityAsync(foodId, isAvailable);
            return Ok(ApiResponse<string>.Ok("Food updated successfully."));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods([FromQuery] FoodQueryParams queryParams)
        {
            var pagination = new PaginationParams
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var paginatedResult = await _foodService.GetFoodsAsync(pagination, queryParams.CategoryId);
            
            return Ok(ApiResponse<PagedResponse<FoodResponseDto>>.Ok(paginatedResult));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string query, [FromQuery]PaginationParams paginationParams)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(ApiResponse<string>.Fail("Search query is required."));
                
            var paginatedResult = await _foodService.SearchFoodsAsync(query,paginationParams);

            return Ok(ApiResponse<PagedResponse<FoodResponseDto>>.Ok(paginatedResult));
        }

        [HttpPost("batch")]
        public async Task<IActionResult> GetByIds([FromBody]List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest(ApiResponse<string>.Fail("A list of food IDs must be provided."));

            var foodIdsDto = await _foodService.GetFoodDetailsByIdsAsync(ids);

            return Ok(ApiResponse<IReadOnlyList<FoodResponseDto>>.Ok(foodIdsDto));
        }

        [HttpGet("Admin-Foods")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminFoods([FromQuery] FoodQueryParams queryParams)
        {
            var pagination = new PaginationParams
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var paginatedResult = await _foodService.GetFoodsForAdminAsync(pagination, queryParams.CategoryId);

            return Ok(ApiResponse<PagedResponse<FoodAdminListDto>>.Ok(paginatedResult));
        }


    }
}