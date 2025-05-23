using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Entities
{
    public class OrderDetail
    { 
        public int Id { get; set; }
        public int FoodId { get; set; }
        public Food? Food { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}