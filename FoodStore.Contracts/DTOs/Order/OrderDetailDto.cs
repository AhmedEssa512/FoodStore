using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Order
{
    public class OrderDetailsDto
    {
        public required string FoodName { get; set; }  
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Quantity * Price; 
    }
}