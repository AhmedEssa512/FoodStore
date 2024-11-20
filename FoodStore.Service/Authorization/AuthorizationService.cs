using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities;
using FoodStore.Data.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FoodStore.Service.Exceptions;

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

        public async Task AddRoleAsync(RoleDto roleDto)
        {

            if(await IsRoleExistByName(roleDto.Name))
             throw new ConflictException("Role is already exist");

            var identityRole = new IdentityRole();

             identityRole.Name = roleDto.Name;

             var result = await _roleManager.CreateAsync(identityRole);

             if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while adding the role.");

        }

        public async Task AddUserToRoleAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDto.Email);

            if(user is null)
             throw new NotFoundException("User is not found");

            if(! await IsRoleExistByName(userRoleDto.roleName))
             throw new NotFoundException("Role is not found");

            if(await _userManager.IsInRoleAsync(user,userRoleDto.roleName))
             throw new ConflictException("User already have this role");

             var result = await _userManager.AddToRoleAsync(user,userRoleDto.roleName);

             if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while adding the user to the role.");
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            
            if(role is null) throw new NotFoundException("Role is not found");

            var result = await _roleManager.DeleteAsync(role);

            if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while removing the role.");

        }

        public async Task DeleteUserFromRoleAsync(UserRoleDto userRoleDto)
        {
           var user = await _userManager.FindByEmailAsync(userRoleDto.Email);

            if(user is null)
             throw new NotFoundException("User is not found");;

            if(! await IsRoleExistByName(userRoleDto.roleName))
             throw new NotFoundException("Role is not found");;

            if(! await _userManager.IsInRoleAsync(user,userRoleDto.roleName))
              throw new ConflictException("User does not have this role");

             var result = await _userManager.RemoveFromRoleAsync(user,userRoleDto.roleName);

             if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while removing the user from the role.");

        }

        public async Task UpdateRoleAsync(string roleId, RoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role is null) throw new NotFoundException("Role is not found");

             role.Name = roleDto.Name;

             var result =  await _roleManager.UpdateAsync(role);

            if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while updating the role.");
              

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