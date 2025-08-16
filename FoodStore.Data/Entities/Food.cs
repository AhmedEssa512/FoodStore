using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Entities
{
    public class Food
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        [Required, MaxLength(500)]
        public required string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public  Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; } = true; 
    }
}