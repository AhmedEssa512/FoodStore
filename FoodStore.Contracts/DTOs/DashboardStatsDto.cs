
namespace FoodStore.Contracts.DTOs
{
    public class DashboardStatsDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalFoods { get; set; }
    }
}