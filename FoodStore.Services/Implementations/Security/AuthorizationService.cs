using FoodStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FoodStore.Services.Exceptions;
using FoodStore.Contracts.DTOs.Auth;
using FoodStore.Contracts.Interfaces.Security;

namespace FoodStore.Services.Implementations.Security
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

        public async Task AddRoleAsync(string roleName)
        {

            if(await IsRoleExistByName(roleName))
             throw new ConflictException("Role is already exist");

            var identityRole = new IdentityRole
            {
                Name = roleName
            };

            var result = await _roleManager.CreateAsync(identityRole);

             if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while adding the role.");

        }

        public async Task AddUserToRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
             throw new NotFoundException("User is not found");

            if(! await IsRoleExistByName(roleName))
             throw new NotFoundException("Role is not found");

            if(await _userManager.IsInRoleAsync(user, roleName))
             throw new ConflictException("User already have this role");

             var result = await _userManager.AddToRoleAsync(user, roleName);

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

        public async Task DeleteUserFromRoleAsync(string email, string roleName)
        {
           var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
             throw new NotFoundException("User is not found");;

            if(! await IsRoleExistByName(roleName))
             throw new NotFoundException("Role is not found");;

            if(! await _userManager.IsInRoleAsync(user, roleName))
              throw new ConflictException("User does not have this role");

             var result = await _userManager.RemoveFromRoleAsync(user, roleName);

             if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while removing the user from the role.");

        }

        public async Task UpdateRoleAsync(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role is null) throw new NotFoundException("Role is not found");

             role.Name = roleName;

             var result =  await _roleManager.UpdateAsync(role);

            if(! result.Succeeded)
              throw new OperationFailedException("An error occurred while updating the role.");
              

        }

        public async Task<IReadOnlyList<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            // Map IdentityRole to RoleDto
            var roleDtos = roles.Select(role => new RoleDto
            {
                // Id = role.Id,
                Name = role.Name ?? string.Empty
            }).ToList();

            return roleDtos;
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