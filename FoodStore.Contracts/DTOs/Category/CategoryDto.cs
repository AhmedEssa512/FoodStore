using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Category
{
    public class CategoryDto
    {
        [Required,MaxLength(20),MinLength(3)]
        public required string Name { get; set; }
    }
}