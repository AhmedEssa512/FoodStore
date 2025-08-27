using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Reports
{
    public class TopFoodDto
    {
        public int Rank { get; set; }        
        public string FoodName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }


        public string RevenueFormatted { get; set; } = string.Empty; 
    }
}