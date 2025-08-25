using FoodStore.Api.Helpers;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Customer;
using FoodStore.Contracts.Interfaces;
using FoodStore.Contracts.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserQueryParams query)
        {
            var customers = await _customerService.GetAllAsync(query);
            return Ok(ApiResponse<PagedResponse<CustomerListDto>>.Ok(customers));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            return Ok(ApiResponse<CustomerInfoDto>.Ok(customer));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _customerService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Ok("User deleted successfully"));
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateUserStatusDto dto)
        {
            await _customerService.UpdateStatusAsync(id, dto);
            return Ok(ApiResponse<string>.Ok("User status updated successfully."));
        }

        
    }
}