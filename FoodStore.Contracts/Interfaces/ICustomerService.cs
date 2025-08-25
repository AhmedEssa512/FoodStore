using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.Common;
using FoodStore.Contracts.DTOs.Customer;
using FoodStore.Contracts.QueryParams;

namespace FoodStore.Contracts.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResponse<CustomerListDto>> GetAllAsync(UserQueryParams query);
        Task<CustomerInfoDto> GetByIdAsync(string id);
         Task UpdateStatusAsync(string id, UpdateUserStatusDto dto);
        Task DeleteAsync(string id);
    }
}