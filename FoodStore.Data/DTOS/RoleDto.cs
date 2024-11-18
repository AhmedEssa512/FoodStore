using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class RoleDto
    {
        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 25 characters.")]
        public string Name { get; set; }
    }
}