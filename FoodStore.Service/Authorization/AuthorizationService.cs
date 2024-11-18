using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Service.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationService(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> AddRoleAsync(RoleDto roleDto)
        {

            if(await IsRoleExistByName(roleDto.Name))
             return "a role is already exist";

            var identityRole = new IdentityRole();

             identityRole.Name = roleDto.Name;

             var result = await _roleManager.CreateAsync(identityRole);

             if(result.Succeeded)
              return "Succeeded";

             return "Failed";
        }

        public async Task<string> AddUserToRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDto.Email);

            if(user is null)
             return "Not found email";

            if(! await IsRoleExistByName(userRoleDto.roleName))
             return "Invaild role";

            if(await _userManager.IsInRoleAsync(user,userRoleDto.roleName))
             return "User already in this role";

             var result = await _userManager.AddToRoleAsync(user,userRoleDto.roleName);

            if(!result.Succeeded) return "Failed";

             return "Succeeded";

        }

        public async Task<string> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            
            if(role is null) return "Not found role";

            var result = await _roleManager.DeleteAsync(role);

            if(!result.Succeeded) return "Failed";

            return "Succeeded";
        }

        public async Task<string> DeleteUserFromRoleAsync(UserRoleDto userRoleDto)
        {
           var user = await _userManager.FindByEmailAsync(userRoleDto.Email);

            if(user is null)
             return "Not found email";

            if(! await IsRoleExistByName(userRoleDto.roleName))
             return "Invaild role";

            if(! await _userManager.IsInRoleAsync(user,userRoleDto.roleName))
             return $"The user does not have {userRoleDto.roleName} role";

             var result = await _userManager.RemoveFromRoleAsync(user,userRoleDto.roleName);

            if(!result.Succeeded) return "Failed";

             return "Succeeded";
        }

        public async Task<string> UpdateRoleAsync(string roleId, RoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role is null) return "Not found role";

             role.Name = roleDto.Name;

             var result =  await _roleManager.UpdateAsync(role);

            if(!result.Succeeded) return "Failed";
           
           return "Succeeded";
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
           return await _roleManager.Roles.ToListAsync();
        }

        public async Task<bool> IsRoleExistById(string roleId)
        {
           var role = await _roleManager.FindByIdAsync(roleId);

           if(role is null) return false;
            

            return true;
        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}