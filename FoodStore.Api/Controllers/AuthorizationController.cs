using FoodStore.Api.Helpers;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly Contracts.Interfaces.Security.IAuthorizationService _authorizationService;

        public AuthorizationController(Contracts.Interfaces.Security.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _authorizationService.GetRolesAsync();
            return Ok(ApiResponse<IReadOnlyList<RoleDto>>.Ok(roles));
        }


        [HttpPost("roles")]
        public async Task<IActionResult> CreateRoleAsync([FromBody]RoleDto roleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));
             
            await _authorizationService.AddRoleAsync(roleDto.Name);
            return Ok(ApiResponse<string>.Ok("Role created successfully"));
        }

        [HttpPut("roles/{roleId}")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId,[FromBody]RoleDto roleDto)
        {

            if(!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

            await _authorizationService.UpdateRoleAsync(roleId, roleDto.Name);
            return Ok(ApiResponse<string>.Ok("Role updated successfully"));
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            await _authorizationService.DeleteRoleAsync(roleId);
            return Ok(ApiResponse<string>.Ok("Role deleted successfully"));
        }

        [HttpPost("roles/AddUserToRole")]
        public async Task<IActionResult> AddUserToRoleAsync([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

             await _authorizationService.AddUserToRoleAsync(userRoleDto.Email , userRoleDto.RoleName);
             return Ok(ApiResponse<string>.Ok("User added to role successfully."));
        }

        [HttpDelete("roles/DeleteUserFromRole")]
        public async Task<IActionResult> DeleteUserFromRoleAsync([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ApiResponseHelper.FromModelState(ModelState));

             await _authorizationService.DeleteUserFromRoleAsync(userRoleDto.Email , userRoleDto.RoleName);
             return Ok(ApiResponse<string>.Ok("User removed from role successfully."));
        }
    }
}