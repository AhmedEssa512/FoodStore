using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Order
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Status { get; set; } = default!;
        public decimal Total { get; set; }
        public string CreatedAt { get; set; } = default!;
    }
}