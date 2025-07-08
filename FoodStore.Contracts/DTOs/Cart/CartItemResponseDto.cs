using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Cart
{
    public class CartItemResponseDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public FoodInCartResponseDto Food { get; set; } = default!;
    }
}