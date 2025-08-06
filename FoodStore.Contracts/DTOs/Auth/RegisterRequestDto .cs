using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs.Auth
{
    public class RegisterRequestDto 
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public required string Email { get; set; }

        
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username must not exceed 50 characters.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 characters.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public required string Password { get; set; }
    }
}