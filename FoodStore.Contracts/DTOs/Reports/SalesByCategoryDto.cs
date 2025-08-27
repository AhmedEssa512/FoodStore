using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Reports
{
    public class SalesByCategoryDto
    {
        public string Category { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal Revenue { get; set; }


        public string RevenueFormatted { get; set; } = string.Empty; // "$12,340.50"
        public double PercentageOfTotal { get; set; } // e.g., 34.6%
    }
}