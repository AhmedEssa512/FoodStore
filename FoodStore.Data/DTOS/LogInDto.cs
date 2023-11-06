using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class LogInDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }       
    }
}