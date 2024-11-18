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
        [Required,MaxLength(20),MinLength(3)]
        public string Name { get; set; }
        [Required,MinLength(50)]
        public string Description { get; set; }
        public virtual ICollection<Food> Foods { get; set; }
    }
}