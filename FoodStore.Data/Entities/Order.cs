using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
// using Microsoft.Identity;

namespace FoodStore.Data.Entities
{
    public class Order
    {

        public Order()
        {
            orderDetails = new List<OrderDetail>();
        }



        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public double Total { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<OrderDetail> orderDetails { get; set; }


    }


    
}