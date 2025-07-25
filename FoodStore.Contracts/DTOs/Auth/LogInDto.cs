using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class LogInDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }       
    }
}