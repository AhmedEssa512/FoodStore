using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Service.DTOS
{
    public class OrderDto
    {
        [Required]
        public string Address { get; set; }
        [Required,MinLength(11)]
        public string Phone { get; set; }
    }
}