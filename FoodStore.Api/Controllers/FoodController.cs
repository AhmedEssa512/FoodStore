using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FoodStore.Data.DTOS;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/foods")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public FoodController(IFoodService foodService, IMapper mapper, IImageService imageService)
        {
            _foodService = foodService;
            _mapper = mapper;
            _imageService = imageService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(FoodDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Create([FromForm]FoodCreateDto foodCreateDto)
        {

            var createdFood = await _foodService.CreateFoodAsync(foodCreateDto);

            var resultDto = _mapper.Map<FoodDto>(createdFood);
        
            return CreatedAtAction(
                   nameof(GetById),  
                   new { id = resultDto.Id },  
                   resultDto  
                 );
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var food = await _foodService.GetFoodAsync(id);

            var resultDto = _mapper.Map<FoodDto>(food);
           
            return Ok(resultDto);
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
        public async Task<IActionResult> Update(int id, [FromForm] FoodUpdateDto foodDto)
        {       
            await _foodService.UpdateFoodAsync(id,foodDto);

            return Ok(new { Message ="Updated successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods([FromQuery] FoodQueryParams queryParams)
        {
            var pagination = new PaginationParams
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var result = await _foodService.GetFoodsAsync(pagination, queryParams.CategoryId);
            var dtoList = _mapper.Map<IEnumerable<FoodDto>>(result.Items);

            var response = new PagedResponse<FoodDto>
            {
                Data = dtoList,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = result.TotalCount
            };
            
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string query, [FromQuery]PaginationParams paginationParams)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required.");
                
            var result = await _foodService.SearchFoodsAsync(query,paginationParams);

            var dtoList = _mapper.Map<IEnumerable<FoodDto>>(result.Items);

             var response = new PagedResponse<FoodDto>
            {
                Data = dtoList,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalCount = result.TotalCount
            };
            
            return Ok(response);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> GetByIds([FromBody]List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return BadRequest("A list of food IDs must be provided.");

            var foods = await _foodService.GetFoodDetailsByIdsAsync(ids);

            var respone = _mapper.Map<IEnumerable<FoodDto>>(foods);
            return Ok(respone);
        }        








    }
}