using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class UserRoleDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}