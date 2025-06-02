using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FoodStore.Contracts.DTOs.Auth
{
    public class RegisterResponseDto : AuthResponseBase
    {
        public bool AccountCreated { get; set; } = true;
        public string? Message { get; set; }
    }
}