using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Food
{
    public class FoodUpdateDto
    {
        [MaxLength(20), MinLength(3)]
        public string? Name { get; set; }
        [MinLength(20)]
        public string? Description { get; set; }
        public double? Price { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
    }
}