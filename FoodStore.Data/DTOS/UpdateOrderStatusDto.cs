using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public required string OrderStatus { get; set; }
    }
}