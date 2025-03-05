using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FoodStore.Data.DTOS
{
    public class FoodDto
    {
        [Required,MaxLength(20),MinLength(3)]
        public string Name { get; set; }
        [Required,MinLength(20)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }

    }
}