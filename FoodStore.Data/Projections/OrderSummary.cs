using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities.Enums;

namespace FoodStore.Data.Projections
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}