using FoodStore.Contracts.DTOs.Auth;

namespace FoodStore.Contracts.Interfaces.Security
{
    public interface IAuthorizationService
    {
        Task AddRoleAsync(string roleName);
        Task<IReadOnlyList<RoleDto>> GetRolesAsync();
        Task<bool> IsRoleExistByName(string roleName);
        Task<bool> IsRoleExistById(string roleId);
        Task UpdateRoleAsync(string roleId, string roleName);
        Task DeleteRoleAsync(string roleId);
        Task AddUserToRoleAsync(string email, string roleName);
        Task DeleteUserFromRoleAsync(string email, string roleName);
    }
}