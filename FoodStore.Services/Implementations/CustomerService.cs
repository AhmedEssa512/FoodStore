using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Customer;
using FoodStore.Contracts.Interfaces;
using FoodStore.Contracts.QueryParams;
using FoodStore.Data.Entities;
using FoodStore.Data.Extensions;
using FoodStore.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
    
        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new OperationFailedException("Failed to delete user");
        }

        public async Task<PagedResponse<CustomerListDto>> GetAllAsync(UserQueryParams query)
        {
            var usersQuery = _userManager.Users.Where(u => u.UserName != null && u.Email != null).AsQueryable();

            // Search by username or email
            if (!string.IsNullOrEmpty(query.Search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.UserName!.Contains(query.Search) ||
                    u.Email!.Contains(query.Search));
            }

            // Filter by IsActive
            if (query.IsActive.HasValue)
            {
                usersQuery = usersQuery.Where(u =>
                    (!u.LockoutEnabled || u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow) 
                    == query.IsActive.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                usersQuery = query.Descending
                    ? usersQuery.OrderByDescending(u => EF.Property<object>(u, query.SortBy))
                    : usersQuery.OrderBy(u => EF.Property<object>(u, query.SortBy));
            }


            // Paging
            var (users, totalCount) = await usersQuery.ToPaginatedListAsync(query.PageNumber,query.PageSize);

            // Map to CustomerListDto
            var userDtos = users.Select(u => new CustomerListDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                Email = u.Email!,
                PhoneNumber = u.PhoneNumber!,
                IsActive = !(u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.UtcNow),
            }).ToList();

            return new PagedResponse<CustomerListDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                PageSize = query.PageSize,
                PageNumber = query.PageNumber
            };
        }

        public async Task<CustomerInfoDto> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new NotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new CustomerInfoDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                IsActive = !(user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow),
                Roles = roles
            };
        }

        public async Task UpdateStatusAsync(string id, UpdateUserStatusDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (dto.IsActive)
            {
                // Make sure user is active
                user.LockoutEnabled = true; // still allow system lockout by failed logins
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;

                if (dto.SuspensionEnd.HasValue)
                {
                    // Temporary suspension
                    user.LockoutEnd = dto.SuspensionEnd.Value;
                }
                else
                {
                    // Permanent ban
                    user.LockoutEnd = DateTimeOffset.MaxValue;
                }
            }

            var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                 throw new OperationFailedException("Failed to update user status.");
        }
    }
}