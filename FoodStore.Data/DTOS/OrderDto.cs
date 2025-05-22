using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Data.DTOS
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Address is required.")]
        public required string Address { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [MinLength(11, ErrorMessage = "Phone number must be at least 11 characters.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]*$", ErrorMessage = "Invalid phone number format.")]
        public required string Phone { get; set; }
    }
}