using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.Entities
{
    public class Category
    {

        public int Id { get; set; }
        [Required,MaxLength(20)]
        public required string Name { get; set; }
        [Required]
        public virtual ICollection<Food> Foods { get; set; } = [];
    }
}