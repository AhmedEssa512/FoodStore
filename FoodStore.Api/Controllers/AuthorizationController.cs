using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using FoodStore.Service.Authorization;
using FoodStore.Service.DTOS;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetRoles")]
        public async Task<IActionResult> getRolesAsync()
        {
            return Ok(await _authorizationService.GetRolesAsync());
        }


        [HttpPost("AddRole")]
        public async Task<IActionResult> addRole(string roleName)
        {
            return Ok(await _authorizationService.AddRoleAsync(roleName));
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> updateRole(string roleId,string roleName)
        {
            return Ok(await _authorizationService.EditRoleAsync(roleId,roleName));
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> deleteRole(string roleId)
        {
            return Ok(await _authorizationService.DeleteRoleAsync(roleId));
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> addUserToRole([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

             return Ok(await _authorizationService.AddUserToRoleAsync(userRoleDto));
        }

        [HttpPost("DeleteUserFromRole")]
        public async Task<IActionResult> DeleteUserFromRole([FromBody] UserRoleDto userRoleDto)
        {
            if(!ModelState.IsValid)
             return BadRequest(ModelState);

             return Ok(await _authorizationService.DeleteUserFromRoleAsync(userRoleDto));
        }
    }
}