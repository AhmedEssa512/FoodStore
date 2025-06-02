using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStore.Contracts.DTOs
{
    public abstract class AuthResponseBase
    {
        public bool IsAuthenticated { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public List<string>? Roles { get; set; }
    }
}