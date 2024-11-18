using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class CartItemDto
    {
        public int FoodId { get; set; }
        public int quantity { get; set; }
    }
}