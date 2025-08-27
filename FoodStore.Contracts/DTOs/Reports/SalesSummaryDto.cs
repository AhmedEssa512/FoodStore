using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Reports
{
    public class SalesSummaryDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItems { get; set; }


        public string TotalSalesFormatted { get; set; } = string.Empty; 
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow; 
    }
}