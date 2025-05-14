using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class OrderDto
    {
        [Required]
        public required string Address { get; set; }
        [Required,MinLength(11)]
        public required string Phone { get; set; }
    }
}