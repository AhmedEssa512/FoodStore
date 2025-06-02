using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class UserRoleDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string RoleName { get; set; }
    }
}