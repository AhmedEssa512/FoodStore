using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using FoodStore.Service.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodStore.Data.DTOS;

namespace FoodStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly Service.Authorization.IAuthorizationService _authorizationService;

        public AuthorizationController(Service.Authorization.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            return Ok(await _authorizationService.GetRolesAsync());
        }


        [HttpPost("roles")]
        public async Task<IActionResult> AddRoleAsync([FromBody]RoleDto roleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);
             
            await _authorizationService.AddRoleAsync(roleDto);

            return Ok(new {Message="Added successfully"});
        }

        [HttpPut("roles/{roleId}")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId,[FromBody]RoleDto roleDto)
        {

            if(!ModelState.IsValid)
             return BadRequest(ModelState);

            await _authorizationService.UpdateRoleAsync(roleId,roleDto);

            return Ok(new {Message="Updated successfully."});
        }

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            await _authorizationService.DeleteRoleAsync(roleId);
            return NoContent();
        }

        [HttpPost("roles/{roleId}/users")]
        public async Task<IActionResult> AddUserToRoleAsync([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

             await _authorizationService.AddUserToRoleAsync(userRoleDto);

             return Ok(new {Message ="Added successfully."});
        }

        [HttpDelete("roles/{roleId}/users")]
        public async Task<IActionResult> DeleteUserFromRoleAsync([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

             await _authorizationService.DeleteUserFromRoleAsync(userRoleDto);

             return NoContent();
        }
    }
}