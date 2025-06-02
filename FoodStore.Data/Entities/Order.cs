using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Data.Entities.Enums;
using Microsoft.AspNetCore.Identity;
// using Microsoft.Identity;

namespace FoodStore.Data.Entities
{
    public class Order
    {

        public int Id { get; set; }
        [Required]
        public required string Address { get; set; }
        [Required]
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal Total { get; set; }
        public required string UserId { get; set; }
        public  ApplicationUser? User { get; set; }
        public  ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    }


    
}