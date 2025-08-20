using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Food
{
    public class FoodAdminListDto
    {
        public int Id { get; set; }
         public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public DateTime CreatedAt { get; set; } 
    }
}
