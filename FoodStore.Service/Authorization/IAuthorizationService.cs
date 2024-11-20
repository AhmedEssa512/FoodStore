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
        Task AddRoleAsync(RoleDto roleDto);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<bool> IsRoleExistByName(string roleName);
        Task<bool> IsRoleExistById(string roleId);
        Task UpdateRoleAsync(string roleId,RoleDto roleDto);
        Task DeleteRoleAsync(string roleId);
        Task AddUserToRoleAsync(UserRoleDto userRoleDto);
        Task DeleteUserFromRoleAsync(UserRoleDto userRoleDto);




    }
}