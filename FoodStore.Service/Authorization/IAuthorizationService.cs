using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.DTOS;
using Microsoft.AspNetCore.Identity;

namespace FoodStore.Service.Authorization
{
    public interface IAuthorizationService
    {
        Task<string> AddRoleAsync(RoleDto roleDto);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<bool> IsRoleExistByName(string roleName);
        Task<bool> IsRoleExistById(string roleId);
        Task<string> UpdateRoleAsync(string roleId,RoleDto roleDto);
        Task<string> DeleteRoleAsync(string roleId);
        Task<string> AddUserToRoleAsync(UserRoleDto userRoleDto);
        Task<string> DeleteUserFromRoleAsync(UserRoleDto userRoleDto);




    }
}