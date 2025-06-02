using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FoodStore.Contracts.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string Status { get; set; }
        public decimal Total { get; set; }
        public required string UserId { get; set; }
        public List<OrderDetailsDto> OrderDetails { get; set; } = new List<OrderDetailsDto>();
    }
}