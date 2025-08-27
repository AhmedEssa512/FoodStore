using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Projections
{
    public class SalesByCategoryProjection
    {
        public string Category { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal Revenue { get; set; }
    }
}