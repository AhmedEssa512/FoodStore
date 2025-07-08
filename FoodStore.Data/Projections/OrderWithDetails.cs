using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities.Enums;

namespace FoodStore.Data.Projections
{
    public class OrderWithDetails
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = default!;
        public decimal Total { get; set; }
        public List<OrderDetailProjection> OrderDetails { get; set; } = new();
    }

    public class OrderDetailProjection
    {
        public string FoodName { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}