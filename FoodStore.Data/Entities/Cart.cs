using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FoodStore.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public  ApplicationUser ? User { get; set; }
        public  ICollection<CartItem> Items { get; set; } = [];
        public decimal Total { get; set; }
    }
}