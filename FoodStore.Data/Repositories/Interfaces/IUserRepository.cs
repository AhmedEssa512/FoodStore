
namespace FoodStore.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> GetUsersCountByRoleAsync(string roleName);
    }
}