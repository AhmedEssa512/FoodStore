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
                return BadRequest("Food data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (file == null || file.Length == 0)
                return BadRequest("Image is required");

            using var stream = file.OpenReadStream();

            var createdFood = await _foodService.CreateFoodAsync(foodCreateDto, stream, file.FileName);
        
            return CreatedAtAction(
                   nameof(GetById),  
                   new { id = createdFood.Id },  
                   createdFood  
                 );
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodDto = await _foodService.GetFoodByIdAsync(id);

            return Ok(foodDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
             await _foodService.DeleteFoodAsync(id);

             return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] FoodUpdateDto foodUpdateDto, IFormFile? file = null)
        {       
            if (foodUpdateDto == null)
                return BadRequest("Food data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Stream? imageStream = null;
            string? originalFileName = null; 

            if(file != null)
            {
               imageStream = file.OpenReadStream();
               originalFileName = file.FileName;
            }

            await _foodService.UpdateFoodAsync(id, foodUpdateDto, imageStream, originalFileName);

            return NoContent();
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
            
            return Ok(paginatedResult);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string query, [FromQuery]PaginationParams paginationParams)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required.");
                
            var paginatedResult = await _foodService.SearchFoodsAsync(query,paginationParams);

            return Ok(paginatedResult);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> GetByIds([FromBody]List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("A list of food IDs must be provided.");

            var responseDtos = await _foodService.GetFoodDetailsByIdsAsync(ids);

            return Ok(responseDtos);
        }


    }
}