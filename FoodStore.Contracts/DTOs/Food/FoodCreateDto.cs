using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Food
{
    public class FoodCreateDto
    {
        [Required,MaxLength(20),MinLength(3)]
        public required string Name { get; set; }
        [Required,MinLength(20)]
        public required string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public required string ImageUrl  { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}