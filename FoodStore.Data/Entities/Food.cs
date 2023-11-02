using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Entities
{
    public class Food
    {
        public int Id { get; set; }
        [Required,MaxLength(20),MinLength(3)]
        public string Name { get; set; }
        [Required,MinLength(50)]
        public string Description { get; set; }
        [Required]
        public double price { get; set; }
        public byte[] Photo { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}