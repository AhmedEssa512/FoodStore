using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Service.DTOS
{
    public class CategoryDto
    {
        [Required,MaxLength(20),MinLength(3)]
        public string Name { get; set; }
        
        [Required,MinLength(50)]
        public string Description { get; set; }
    }
}