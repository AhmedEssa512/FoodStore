using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // unit price
        public int CartId { get; set; }
        public  Cart? Cart { get; set; }
        public int FoodId { get; set; }
        public Food? Food { get; set; }
    }
}