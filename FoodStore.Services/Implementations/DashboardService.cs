using FoodStore.Contracts.DTOs;
using FoodStore.Contracts.Interfaces;
using FoodStore.Data.Repositories.Interfaces;

namespace FoodStore.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalSales = await _unitOfWork.Order.GetTotalSalesAsync();
            var totalOrders = await _unitOfWork.Order.GetTotalOrdersAsync();
            var totalFoods = await _unitOfWork.Food.GetTotalFoodssAsync();
            var totalCustomers = await _unitOfWork.User.GetUsersCountByRoleAsync("Customer");

            return new DashboardStatsDto
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                TotalFoods = totalFoods
            };

        }
    }
}