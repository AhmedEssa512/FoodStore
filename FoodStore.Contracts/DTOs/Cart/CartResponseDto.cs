using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Cart
{
    public class CartResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}