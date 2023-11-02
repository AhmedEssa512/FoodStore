using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Service.DTOS
{
    public class UserRoleDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}