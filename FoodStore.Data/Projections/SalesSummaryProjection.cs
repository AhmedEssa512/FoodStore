using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Projections
{
    public class SalesSummaryProjection
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItems { get; set; }
    }
}