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
        public int Amount { get; set; }
        public int foodId { get; set; }
        public Food food { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}