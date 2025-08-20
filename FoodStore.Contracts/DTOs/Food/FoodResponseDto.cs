using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Food
{
    public class FoodResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public  string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }
    }
}